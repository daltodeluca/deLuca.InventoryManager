using deLuca.InventoryManager.Application.DTOs;

namespace deLuca.InventoryManager.Application.Services;

public interface IAuthService
{
    string? Authenticate(LoginRequest request, CancellationToken cancellationToken = default);
}
