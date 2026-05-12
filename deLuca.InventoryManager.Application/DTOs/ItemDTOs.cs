namespace deLuca.InventoryManager.Application.DTOs;

public sealed record ItemResponse(int Id, string CodigoFormatado, string Descricao, string SubcategoriaNome);

public sealed record PagedResponse<T>(IEnumerable<T> Data, int CurrentPage, int PageSize, int TotalItems)
{
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
}
