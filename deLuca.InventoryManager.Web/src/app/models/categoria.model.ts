export interface CategoriaResponse {
  id: number;
  nome: string;
  prefixo: string;
}

export interface CreateCategoriaRequest {
  nome: string;
  prefixo: string;
}
