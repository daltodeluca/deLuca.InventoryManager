using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Categorias.Queries;

public sealed record GetAllCategoriasQuery : IRequest<IEnumerable<CategoriaResponse>>;

public sealed class GetAllCategoriasQueryHandler(ICategoriaRepository repository)
    : IRequestHandler<GetAllCategoriasQuery, IEnumerable<CategoriaResponse>>
{
    public async Task<IEnumerable<CategoriaResponse>> Handle(
        GetAllCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await repository.GetAllAsync(cancellationToken);
        return categorias.Select(c => c.ToResponse());
    }
}
