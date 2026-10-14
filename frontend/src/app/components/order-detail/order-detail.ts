import { AsyncPipe, CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, filter, Observable, switchMap } from 'rxjs';
import { Customer } from '../../models/customer.model';
import { Order } from '../../models/order.model';
import { Product } from '../../models/product.model';
import { CustomerService } from '../../services/customer.service';
import { OrderService } from '../../services/order.service';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, AsyncPipe, DatePipe, DecimalPipe],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css'
})
export class OrderDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly orderService = inject(OrderService);
  private readonly customerService = inject(CustomerService);
  private readonly productService = inject(ProductService);

  customers = signal<Customer[]>([]);
  products = signal<Product[]>([]);

  private readonly orderId$ = new BehaviorSubject<string | null>(null);
  order$: Observable<Order> = this.orderId$.pipe(
    filter((id): id is string => !!id),
    switchMap(id => this.orderService.getById(id))
  );

  lineForm = this.fb.group({
    productId: ['', Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]],
    unitPrice: [0, [Validators.required, Validators.min(0)]]
  });

  showLineForm = false;
  working = false;
  error: string | null = null;

  ngOnInit(): void {
    this.customerService.getAll().subscribe(customers => this.customers.set(customers));
    this.productService.getAll().subscribe(products => this.products.set(products));

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) this.orderId$.next(id);
    });
  }

  get currentId(): string {
    return this.orderId$.value!;
  }

  customerName(customerId: string): string {
    return this.customers().find(customer => customer.id === customerId)?.name ?? customerId;
  }

  productName(productId: string): string {
    return this.products().find(product => product.id === productId)?.name ?? productId;
  }

  productSku(productId: string): string {
    return this.products().find(product => product.id === productId)?.sku ?? '';
  }

  addLine(): void {
    if (this.lineForm.invalid) return;
    this.working = true;
    this.error = null;
    const value = this.lineForm.value;
    this.orderService.addLine(this.currentId, {
      productId: value.productId!,
      quantity: value.quantity!,
      unitPrice: value.unitPrice!
    }).subscribe({
      next: () => { this.working = false; this.showLineForm = false; this.lineForm.reset({ quantity: 1, unitPrice: 0 }); this.reload(); },
      error: err => { this.working = false; this.error = err?.error?.error ?? 'Could not add line.'; }
    });
  }

  confirm(): void { this.runAction(() => this.orderService.confirm(this.currentId)); }
  ship(): void { this.runAction(() => this.orderService.ship(this.currentId)); }
  cancel(): void { this.runAction(() => this.orderService.cancel(this.currentId)); }

  private runAction(call: () => Observable<Order>): void {
    this.working = true;
    this.error = null;
    call().subscribe({
      next: () => { this.working = false; this.reload(); },
      error: err => { this.working = false; this.error = err?.error?.error ?? 'Action failed.'; }
    });
  }

  private reload(): void {
    const id = this.orderId$.value;
    if (id) this.orderId$.next(id);
  }
}
