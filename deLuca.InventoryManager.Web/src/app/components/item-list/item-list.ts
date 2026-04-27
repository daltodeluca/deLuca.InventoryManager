import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemService } from '../../services/item';
import { ItemResponse } from '../../models/item.model';

@Component({
  selector: 'app-item-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './item-list.html',
  styleUrl: './item-list.css'
})
export class ItemList {
  private itemService = inject(ItemService);
  
  itens: ItemResponse[] = [];
  statusMessage = 'Aguardando ação...';
  isLoading = false;

  loadItens() {
    this.isLoading = true;
    this.statusMessage = 'A carregar itens...';
    
    this.itemService.getItens().subscribe({
      next: (resposta) => {
        this.itens = resposta.data; 
        
        this.statusMessage = `Sucesso! Recebidos ${resposta.totalItems} produtos.`; 
        this.isLoading = false;
      },
      error: (erro) => {
        console.error(erro);
        this.statusMessage = 'Erro na chamada! Veja o Console (F12).';
        this.isLoading = false;
      }
    });
  }
}