import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SubcategoriaService } from '../../services/subcategoria';
import { CategoriaService } from '../../services/categoria';
import { SubcategoriaResponse } from '../../models/subcategoria.model';
import { CategoriaResponse } from '../../models/categoria.model';

@Component({
  selector: 'app-subcategoria-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './subcategoria-list.html',
  styleUrl: './subcategoria-list.css'
})
export class SubcategoriaList implements OnInit {
  private subService = inject(SubcategoriaService);
  private catService = inject(CategoriaService);

  subcategorias: SubcategoriaResponse[] = [];
  
  categorias: CategoriaResponse[] = [];
  
  novaSub = { nome: '', categoriaId: 0 };
  statusMessage = '';
  isSaving = false;

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.subService.getSubcategorias().subscribe(res => this.subcategorias = res);
    this.catService.getCategorias().subscribe(res => this.categorias = res);
  }

  salvar() {
    if (!this.novaSub.nome || this.novaSub.categoriaId === 0) return;
    this.isSaving = true;
    this.subService.addSubcategoria(this.novaSub).subscribe({
      next: (res) => {
        this.subcategorias.unshift(res);
        this.novaSub = { nome: '', categoriaId: 0 };
        this.isSaving = false;
        this.statusMessage = 'Subcategoria adicionada!';
      },
      error: () => {
        this.isSaving = false;
        this.statusMessage = 'Erro ao salvar.';
      }
    });
  }

  excluir(id: number) {
    if (confirm('Deseja desativar esta subcategoria?')) {
      this.subService.deleteSubcategoria(id).subscribe({
        next: () => {
          this.subcategorias = this.subcategorias.filter(s => s.id !== id);
        },
        error: (err) => console.error(err)
      });
    }
  }
}