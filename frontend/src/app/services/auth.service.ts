import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { LoginRequestDto, LoginResponseDto } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storageKey = 'projectnet.token';
  private readonly usernameKey = 'projectnet.username';
  private readonly roleKey = 'projectnet.role';

  token = signal<string | null>(localStorage.getItem(this.storageKey));
  username = signal<string | null>(localStorage.getItem(this.usernameKey));
  role = signal<string | null>(localStorage.getItem(this.roleKey));

  isLoggedIn = computed(() => !!this.token());
  isAdmin = computed(() => this.role() === 'Admin');

  login(username: string, password: string) {
    return this.http.post<LoginResponseDto>('http://localhost:5000/api/auth/login', { username, password }).pipe(
      tap(response => {
        this.token.set(response.token);
        this.username.set(response.username);
        this.role.set(response.role);
        localStorage.setItem(this.storageKey, response.token);
        localStorage.setItem(this.usernameKey, response.username);
        localStorage.setItem(this.roleKey, response.role);
      })
    );
  }

  logout(): void {
    this.token.set(null);
    this.username.set(null);
    this.role.set(null);
    localStorage.removeItem(this.storageKey);
    localStorage.removeItem(this.usernameKey);
    localStorage.removeItem(this.roleKey);
    this.router.navigate(['/login']);
  }
}
