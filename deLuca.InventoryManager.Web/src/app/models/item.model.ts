export interface ItemResponse {
  id: number;
  codigoFormatado: string;
  descricao: string;
  subcategoriaNome: string;
}

export interface PaginatedItemResponse {
  data: ItemResponse[];
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}