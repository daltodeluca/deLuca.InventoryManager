import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ItemService } from '../../services/item';
import { SubcategoriaService } from '../../services/subcategoria';
import { CategoriaService } from '../../services/categoria'; // Importação nova
import { ItemResponse } from '../../models/item.model';
import { SubcategoriaResponse } from '../../models/subcategoria.model';
import { CategoriaResponse } from '../../models/categoria.model';

@Component({
  selector: 'app-item-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './item-list.html',
  styleUrl: './item-list.css'
})
export class ItemList implements OnInit {
  private itemService = inject(ItemService);
  private subService = inject(SubcategoriaService);
  private catService = inject(CategoriaService);

  itens: ItemResponse[] = [];
  categorias: CategoriaResponse[] = [];
  todasSubcategorias: SubcategoriaResponse[] = [];
  subcategoriasFiltradas: SubcategoriaResponse[] = [];
  
  categoriaSelecionadaId: number = 0;
  novoItem = { codigoFormatado: '', descricao: '', subcategoriaId: 0 };
  isSaving = false;

  ngOnInit() {
    this.loadInitialData();
  }

  loadInitialData() {
    this.itemService.getItens().subscribe(res => this.itens = res.data);
    this.catService.getCategorias().subscribe(res => this.categorias = res);
    this.subService.getSubcategorias().subscribe(res => this.todasSubcategorias = res);
  }

  onCategoriaChange() {
    this.subcategoriasFiltradas = this.todasSubcategorias.filter(
      s => s.categoriaId == this.categoriaSelecionadaId
    );
    this.novoItem.subcategoriaId = 0;
  }

  salvar() {
    this.isSaving = true;
    this.itemService.addItem(this.novoItem).subscribe({
      next: (res) => {
        this.itens.unshift(res);
        this.novoItem = { codigoFormatado: '', descricao: '', subcategoriaId: 0 };
        this.categoriaSelecionadaId = 0;
        this.subcategoriasFiltradas = [];
        this.isSaving = false;
      },
      error: () => this.isSaving = false
    });
  }

  excluir(id: number) {
    if (confirm('Deseja desativar este item?')) {
      this.itemService.deleteItem(id).subscribe(() => {
        this.itens = this.itens.filter(i => i.id !== id);
      });
    }
  }
}