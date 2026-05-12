using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Itens.Queries;

public sealed record GetItemByIdQuery(int Id) : IRequest<ItemResponse?>;

public sealed class GetItemByIdQueryHandler(IItemTecnologiaRepository repository)
    : IRequestHandler<GetItemByIdQuery, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(
        GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        return item?.ToResponse();
    }
}
