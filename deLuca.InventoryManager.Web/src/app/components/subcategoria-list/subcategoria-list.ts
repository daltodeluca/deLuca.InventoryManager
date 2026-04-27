import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubcategoriaService } from '../../services/subcategoria';
import { SubcategoriaResponse } from '../../models/subcategoria.model';

@Component({
  selector: 'app-subcategoria-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subcategoria-list.html',
  styleUrl: './subcategoria-list.css'
})
export class SubcategoriaList {
  private subcategoriaService = inject(SubcategoriaService);
  
  subcategorias: SubcategoriaResponse[] = [];
  statusMessage = 'Aguardando ação...';
  isLoading = false;

  loadSubcategorias() {
    this.isLoading = true;
    this.statusMessage = 'A carregar subcategorias...';
    
    this.subcategoriaService.getSubcategorias().subscribe({
      next: (dados) => {
        this.subcategorias = dados;
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