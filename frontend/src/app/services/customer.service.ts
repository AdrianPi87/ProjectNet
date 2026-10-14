import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private readonly apiUrl = 'http://localhost:5000/api/customers';
  constructor(private readonly http: HttpClient) {}
  getAll(): Observable<Customer[]> { return this.http.get<Customer[]>(this.apiUrl); }
  getById(id: string): Observable<Customer> { return this.http.get<Customer>(this.apiUrl + '/' + id); }
  create(payload: { name: string; email: string }): Observable<Customer> {
    return this.http.post<Customer>(this.apiUrl, payload);
  }
}
