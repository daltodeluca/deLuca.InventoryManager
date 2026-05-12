using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Subcategorias.Queries;

public sealed record GetSubcategoriaByIdQuery(int Id) : IRequest<SubcategoriaResponse?>;

public sealed class GetSubcategoriaByIdQueryHandler(ISubcategoriaRepository repository)
    : IRequestHandler<GetSubcategoriaByIdQuery, SubcategoriaResponse?>
{
    public async Task<SubcategoriaResponse?> Handle(
        GetSubcategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var sub = await repository.GetByIdAsync(request.Id, cancellationToken);
        return sub?.ToResponse();
    }
}
