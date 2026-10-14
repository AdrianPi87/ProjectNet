import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly apiUrl = 'http://localhost:5000/api/products';
  constructor(private readonly http: HttpClient) {}
  getAll(): Observable<Product[]> { return this.http.get<Product[]>(this.apiUrl); }
  getById(id: string): Observable<Product> { return this.http.get<Product>(this.apiUrl + '/' + id); }
  create(payload: { name: string; sku: string; category: string; unitPrice: number }): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, payload);
  }
}
