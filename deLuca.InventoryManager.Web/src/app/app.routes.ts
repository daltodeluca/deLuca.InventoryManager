import { Routes } from '@angular/router';
import { CategoriaListComponent } from './components/categoria-list/categoria-list';
import { SubcategoriaList } from './components/subcategoria-list/subcategoria-list';
import { ItemList } from './components/item-list/item-list';

export const routes: Routes = [
  { path: 'categorias', component: CategoriaListComponent },
  
  { path: 'subcategorias', component: SubcategoriaList },
  
  { path: 'itens', component: ItemList },
  
  { path: '', redirectTo: '/categorias', pathMatch: 'full' } 
];