using deLuca.InventoryManager.Domain.Entities;

namespace deLuca.InventoryManager.Domain.Interfaces;

public interface ISubcategoriaRepository
{
    Task<IEnumerable<Subcategoria>> GetAllAsync(CancellationToken ct = default);
    Task<Subcategoria?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> AddAsync(Subcategoria subcategoria, CancellationToken ct = default);
    Task<bool> UpdateAsync(Subcategoria subcategoria, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
