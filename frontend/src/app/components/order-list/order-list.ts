import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Customer } from '../../models/customer.model';
import { Order } from '../../models/order.model';
import { CustomerService } from '../../services/customer.service';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [RouterLink, DatePipe, DecimalPipe, FormsModule],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css'
})
export class OrderListComponent implements OnInit {
  private readonly orderService = inject(OrderService);
  private readonly customerService = inject(CustomerService);

  customers = signal<Customer[]>([]);
  orders = signal<Order[]>([]);
  page = signal(1);
  pageSize = signal(10);
  selectedStatus = signal<'ALL' | 'Pending' | 'Confirmed' | 'Shipped' | 'Cancelled'>('ALL');
  selectedCustomerId = signal<string>('');
  totalPages = signal(1);
  totalCount = signal(0);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.customerService.getAll().subscribe(customers => this.customers.set(customers));
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading.set(true);
    this.error.set(null);

    const status = this.selectedStatus() === 'ALL' ? undefined : this.selectedStatus();
    const customerId = this.selectedCustomerId() || undefined;

    this.orderService.getAll(this.page(), this.pageSize(), status, customerId).subscribe({
      next: response => {
        this.orders.set(response.items);
        this.totalCount.set(response.totalCount);
        this.totalPages.set(response.totalPages || 1);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Could not load orders.');
      }
    });
  }

  changeStatus(value: string): void {
    this.selectedStatus.set(value as any);
    this.page.set(1);
    this.loadOrders();
  }

  changeCustomer(value: string): void {
    this.selectedCustomerId.set(value);
    this.page.set(1);
    this.loadOrders();
  }

  changePageSize(value: string): void {
    this.pageSize.set(Number(value));
    this.page.set(1);
    this.loadOrders();
  }

  previousPage(): void {
    if (this.page() > 1) {
      this.page.update(current => current - 1);
      this.loadOrders();
    }
  }

  nextPage(): void {
    if (this.page() < this.totalPages()) {
      this.page.update(current => current + 1);
      this.loadOrders();
    }
  }

  customerName(customerId: string): string {
    return this.customers().find(customer => customer.id === customerId)?.name ?? customerId;
  }
}


