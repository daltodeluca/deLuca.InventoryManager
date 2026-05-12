namespace deLuca.InventoryManager.Domain.Entities;

public sealed class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Prefixo { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
