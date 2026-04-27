using System.Collections.Generic;
using System.Linq;

namespace deLuca.InventoryManager.Api.DTOs;

public record CategoriaResponse(int Id, string Nome, string Prefixo);
public record CreateCategoriaRequest(string Nome, string Prefixo);

public record SubcategoriaResponse(int Id, string Nome, int CategoriaId, string CategoriaNome);
public record CreateSubcategoriaRequest(string Nome, int CategoriaId);

public record ItemResponse(int Id, string CodigoFormatado, string Descricao, string SubcategoriaNome);
public record CreateItemRequest(string CodigoFormatado, string Descricao, int SubcategoriaId);

public record UpdateCategoriaRequest(string Nome, string Prefixo);
public record UpdateSubcategoriaRequest(string Nome, int CategoriaId);
public record UpdateItemRequest(string CodigoFormatado, string Descricao, int SubcategoriaId);

public record LoginRequest(string Email, string Password);

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
}
