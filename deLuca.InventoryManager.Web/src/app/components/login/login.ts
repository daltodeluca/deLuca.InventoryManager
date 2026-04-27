import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  credentials = {
    email: '',
    password: ''
  };

  statusMessage = '';
  isLoading = false;

  efetuarLogin() {
    if (!this.credentials.email || !this.credentials.password) {
      this.statusMessage = 'Preencha todos os campos.';
      return;
    }

    this.isLoading = true;
    this.statusMessage = 'Autenticando...';

    this.authService.login(this.credentials).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/categorias']);
      },
      error: (err: any) => {
        this.isLoading = false;
        this.statusMessage = 'E-mail ou senha inválidos.';
        console.error(err);
      }
    });
  }
}