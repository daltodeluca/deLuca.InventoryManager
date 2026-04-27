import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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
}