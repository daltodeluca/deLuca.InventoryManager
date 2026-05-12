namespace deLuca.InventoryManager.Domain.Entities;

public sealed class Subcategoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public bool Ativo { get; set; } = true;

    // Populated by JOIN queries in the repository — not persisted
    public string? CategoriaNome { get; set; }
}
