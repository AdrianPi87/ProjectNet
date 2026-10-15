import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserAccountDto } from '../../models/auth.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, DatePipe],
  templateUrl: './admin-users.html',
  styleUrl: './admin-users.css'
})
export class AdminUsersComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);

  users = signal<UserAccountDto[]>([]);
  submitting = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  form = this.fb.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]],
    role: ['User', Validators.required]
  });

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAll().subscribe((users: UserAccountDto[]) => this.users.set(users));
  }

  submit(): void {
    if (this.form.invalid || this.submitting()) return;
    this.submitting.set(true);
    this.error.set(null);
    const value = this.form.getRawValue();
    this.userService.create({ username: value.username!, password: value.password!, role: value.role! }).subscribe({
      next: (user: UserAccountDto) => {
        this.submitting.set(false);
        this.success.set(`User created: ${user.username} (${user.role})`);
        this.form.reset({ role: 'User' });
        this.loadUsers();
      },
      error: (err: any) => {
        this.submitting.set(false);
        this.error.set(err?.error?.error ?? err?.error ?? 'Could not create user.');
      }
    });
  }
}




