import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemResponse } from '../models/item.model';
import { PaginatedItemResponse } from '../models/item.model';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private http = inject(HttpClient);
  
  private apiUrl = 'http://localhost:5235/api/itens'; 

  getItens(): Observable<PaginatedItemResponse> {
    return this.http.get<PaginatedItemResponse>(this.apiUrl);
  }

  addItem(dados: { codigoFormatado: string; descricao: string; subcategoriaId: number }): Observable<ItemResponse> {
    return this.http.post<ItemResponse>(this.apiUrl, dados);
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}