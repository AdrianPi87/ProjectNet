import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Customer } from '../../models/customer.model';
import { CustomerService } from '../../services/customer.service';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './create-order.html',
  styleUrl: './create-order.css'
})
export class CreateOrderComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly orderService = inject(OrderService);
  private readonly customerService = inject(CustomerService);
  private readonly router = inject(Router);

  customers = signal<Customer[]>([]);
  submitting = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    customerId: ['', Validators.required]
  });

  ngOnInit(): void {
    this.customerService.getAll().subscribe(customers => this.customers.set(customers));
  }

  submit(): void {
    if (this.form.invalid || this.submitting()) return;
    this.submitting.set(true);
    this.error.set(null);
    const { customerId } = this.form.getRawValue();
    this.orderService.create({ customerId: customerId! }).subscribe({
      next: order => {
        this.submitting.set(false);
        this.router.navigate(['/orders', order.id]);
      },
      error: err => {
        this.submitting.set(false);
        this.error.set(err?.error?.error ?? 'Could not create order.');
      }
    });
  }
}
