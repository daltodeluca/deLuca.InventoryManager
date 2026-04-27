import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SubcategoriaResponse } from '../models/subcategoria.model';

@Injectable({
  providedIn: 'root'
})
export class SubcategoriaService {
  private http = inject(HttpClient);
  
  private apiUrl = 'http://localhost:5235/api/subcategorias'; 

  getSubcategorias(): Observable<SubcategoriaResponse[]> {
    return this.http.get<SubcategoriaResponse[]>(this.apiUrl);
  }

  addSubcategoria(dados: { nome: string; categoriaId: number }): Observable<SubcategoriaResponse> {
    return this.http.post<SubcategoriaResponse>(this.apiUrl, dados);
  }
  
  deleteSubcategoria(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}