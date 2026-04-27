import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriaService } from '../../services/categoria';
import { CategoriaResponse } from '../../models/categoria.model';

@Component({
  selector: 'app-categoria-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './categoria-list.html',
  styleUrl: './categoria-list.css'
})
export class CategoriaListComponent {
  private categoriaService = inject(CategoriaService);
  
  categorias: CategoriaResponse[] = [];
  statusMessage = 'Aguardando ação...';
  isLoading = false;

  loadCategorias() {
    this.isLoading = true;
    this.statusMessage = 'A chamar a API...';
    
    this.categoriaService.getCategorias().subscribe({
      next: (dados) => {
        this.categorias = dados;
        this.statusMessage = `Sucesso! Recebidos ${dados.length} itens.`;
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