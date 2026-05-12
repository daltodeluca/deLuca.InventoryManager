using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Subcategorias.Queries;

public sealed record GetAllSubcategoriasQuery : IRequest<IEnumerable<SubcategoriaResponse>>;

public sealed class GetAllSubcategoriasQueryHandler(ISubcategoriaRepository repository)
    : IRequestHandler<GetAllSubcategoriasQuery, IEnumerable<SubcategoriaResponse>>
{
    public async Task<IEnumerable<SubcategoriaResponse>> Handle(
        GetAllSubcategoriasQuery request, CancellationToken cancellationToken)
    {
        var subcategorias = await repository.GetAllAsync(cancellationToken);
        return subcategorias.Select(s => s.ToResponse());
    }
}
