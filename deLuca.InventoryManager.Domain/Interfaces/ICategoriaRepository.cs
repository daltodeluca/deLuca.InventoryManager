using deLuca.InventoryManager.Domain.Entities;

namespace deLuca.InventoryManager.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync(CancellationToken ct = default);
    Task<Categoria?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> AddAsync(Categoria categoria, CancellationToken ct = default);
    Task<bool> UpdateAsync(Categoria categoria, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
