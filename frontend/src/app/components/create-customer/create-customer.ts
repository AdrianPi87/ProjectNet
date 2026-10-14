import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';

@Component({
  selector: 'app-create-customer',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './create-customer.html',
  styleUrl: './create-customer.css'
})
export class CreateCustomerComponent {
  private readonly fb = inject(FormBuilder);
  private readonly customerService = inject(CustomerService);
  private readonly router = inject(Router);

  submitting = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  form = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]]
  });

  submit(): void {
    if (this.form.invalid || this.submitting()) return;
    this.submitting.set(true);
    this.error.set(null);

    const { name, email } = this.form.getRawValue();
    this.customerService.create({ name: name!, email: email! }).subscribe({
      next: customer => {
        this.submitting.set(false);
        this.success.set(`Customer created: ${customer.name}`);
        this.form.reset();
        this.router.navigate(['/']);
      },
      error: err => {
        this.submitting.set(false);
        this.error.set(err?.error?.error ?? 'Could not create customer.');
      }
    });
  }
}
