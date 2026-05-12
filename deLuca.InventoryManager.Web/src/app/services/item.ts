import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemResponse, PagedResponse, CreateItemRequest } from '../models/item.model';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5235/api/itens';

  getItens(page = 1, pageSize = 10): Observable<PagedResponse<ItemResponse>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResponse<ItemResponse>>(this.apiUrl, { params });
  }

  getItemById(id: number): Observable<ItemResponse> {
    return this.http.get<ItemResponse>(`${this.apiUrl}/detalhes/${id}`);
  }

  addItem(dados: CreateItemRequest): Observable<ItemResponse> {
    return this.http.post<ItemResponse>(this.apiUrl, dados);
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
