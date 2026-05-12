using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Categorias.Queries;

public sealed record GetCategoriaByIdQuery(int Id) : IRequest<CategoriaResponse?>;

public sealed class GetCategoriaByIdQueryHandler(ICategoriaRepository repository)
    : IRequestHandler<GetCategoriaByIdQuery, CategoriaResponse?>
{
    public async Task<CategoriaResponse?> Handle(
        GetCategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var categoria = await repository.GetByIdAsync(request.Id, cancellationToken);
        return categoria?.ToResponse();
    }
}
