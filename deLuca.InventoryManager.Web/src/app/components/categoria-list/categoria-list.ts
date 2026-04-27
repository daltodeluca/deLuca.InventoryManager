import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoriaService } from '../../services/categoria';
import { CategoriaResponse } from '../../models/categoria.model';

@Component({
  selector: 'app-categoria-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categoria-list.html',
  styleUrl: './categoria-list.css'
})
export class CategoriaListComponent implements OnInit {
  private categoriaService = inject(CategoriaService);
  
  categorias: CategoriaResponse[] = [];
  statusMessage = '';
  isLoading = false;
  isSaving = false;
  novaCategoria = { nome: '', prefixo: '' };

  ngOnInit() {
    this.loadCategorias();
  }

  loadCategorias() {
    this.isLoading = true;
    this.categoriaService.getCategorias().subscribe({
      next: (dados) => {
        this.categorias = dados;
        this.isLoading = false;
      },
      error: () => this.isLoading = false
    });
  }

  salvarCategoria() {
    if (!this.novaCategoria.nome || !this.novaCategoria.prefixo) return;
    this.isSaving = true;
    this.categoriaService.addCategoria(this.novaCategoria).subscribe({
      next: (res) => {
        this.categorias.unshift(res);
        this.novaCategoria = { nome: '', prefixo: '' };
        this.isSaving = false;
      },
      error: () => this.isSaving = false
    });
  }

  excluirCategoria(id: number) {
    if (confirm('Deseja realmente excluir?')) {
      this.categoriaService.deleteCategoria(id).subscribe({
        next: () => {
          this.categorias = this.categorias.filter(c => c.id !== id);
          this.statusMessage = 'Excluído com sucesso!';
        },
        error: (err) => console.error(err)
      });
    }
  }
}