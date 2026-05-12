export interface SubcategoriaResponse {
  id: number;
  nome: string;
  categoriaId: number;
  categoriaNome: string;
}

export interface CreateSubcategoriaRequest {
  nome: string;
  categoriaId: number;
}
