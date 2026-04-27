import { Routes } from '@angular/router';
import { CategoriaListComponent } from './components/categoria-list/categoria-list';
import { SubcategoriaList } from './components/subcategoria-list/subcategoria-list';
import { ItemList } from './components/item-list/item-list';
import { LoginComponent } from './components/login/login';

export const routes: Routes = [
  { path: 'categorias', component: CategoriaListComponent },
  
  { path: 'subcategorias', component: SubcategoriaList },
  
  { path: 'itens', component: ItemList },

  { path: 'login', component: LoginComponent },
  
  { path: '', redirectTo: '/categorias', pathMatch: 'full' } 
];