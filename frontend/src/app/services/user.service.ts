import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserAccountDto } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly apiUrl = 'http://localhost:5000/api/users';
  constructor(private readonly http: HttpClient) {}
  getAll(): Observable<UserAccountDto[]> { return this.http.get<UserAccountDto[]>(this.apiUrl); }
  create(payload: { username: string; password: string; role: string }): Observable<UserAccountDto> {
    return this.http.post<UserAccountDto>(this.apiUrl, payload);
  }
}
