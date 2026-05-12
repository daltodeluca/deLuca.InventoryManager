namespace deLuca.InventoryManager.Domain.Entities;

public sealed class ItemTecnologia
{
    public int Id { get; set; }
    public string CodigoFormatado { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int SubcategoriaId { get; set; }
    public bool Ativo { get; set; } = true;

    // Populated by JOIN queries in the repository — not persisted
    public string? SubcategoriaNome { get; set; }
}
