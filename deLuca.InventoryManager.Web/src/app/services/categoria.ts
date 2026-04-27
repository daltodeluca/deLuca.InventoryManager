import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CategoriaResponse } from '../models/categoria.model';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {
  private http = inject(HttpClient);
  
  private apiUrl = 'http://localhost:5235/api/categorias'; 

  getCategorias(): Observable<CategoriaResponse[]> {
    return this.http.get<CategoriaResponse[]>(this.apiUrl);
  }
}