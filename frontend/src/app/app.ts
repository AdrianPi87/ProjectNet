import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  protected readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  get loggedIn() { return this.auth.isLoggedIn(); }
  get admin() { return this.auth.isAdmin(); }
  get username() { return this.auth.username(); }

  logout(): void { this.auth.logout(); }
}
