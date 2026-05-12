export interface ItemResponse {
  id: number;
  codigoFormatado: string;
  descricao: string;
  subcategoriaNome: string;
}

export interface PagedResponse<T> {
  data: T[];
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

export interface CreateItemRequest {
  codigoFormatado: string;
  descricao: string;
  subcategoriaId: number;
}
