import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddOrderLineRequest, CreateOrderRequest, Order, OrdersPage } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly apiUrl = 'http://localhost:5000/api/orders';

  constructor(private readonly http: HttpClient) {}

  getAll(page = 1, pageSize = 10, status?: string, customerId?: string): Observable<OrdersPage> {
    let params = new HttpParams().set('page', page).set('pageSize', pageSize);
    if (status) params = params.set('status', status);
    if (customerId) params = params.set('customerId', customerId);
    return this.http.get<OrdersPage>(this.apiUrl, { params });
  }

  getById(id: string): Observable<Order> { return this.http.get<Order>(`${this.apiUrl}/${id}`); }

  create(request: CreateOrderRequest): Observable<Order> { return this.http.post<Order>(this.apiUrl, request); }

  addLine(orderId: string, request: AddOrderLineRequest): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/${orderId}/lines`, request);
  }

  confirm(orderId: string): Observable<Order> { return this.http.post<Order>(`${this.apiUrl}/${orderId}/confirm`, {}); }
  ship(orderId: string): Observable<Order> { return this.http.post<Order>(`${this.apiUrl}/${orderId}/ship`, {}); }
  cancel(orderId: string): Observable<Order> { return this.http.post<Order>(`${this.apiUrl}/${orderId}/cancel`, {}); }
}
