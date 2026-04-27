import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5235/api/auth/login';

  login(credentials: any) {
    return this.http.post<{ token: string }>(this.apiUrl, credentials).pipe(
      tap(res => {
        localStorage.setItem('inventory_token', res.token);
      })
    );
  }

  getToken() {
    return localStorage.getItem('inventory_token');
  }

  logout() {
    localStorage.removeItem('inventory_token');
  }
}