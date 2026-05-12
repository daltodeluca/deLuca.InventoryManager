using deLuca.InventoryManager.Domain.Entities;

namespace deLuca.InventoryManager.Domain.Interfaces;

public interface IItemTecnologiaRepository
{
    Task<IEnumerable<ItemTecnologia>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<ItemTecnologia?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> AddAsync(ItemTecnologia item, CancellationToken ct = default);
    Task<bool> UpdateAsync(ItemTecnologia item, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
