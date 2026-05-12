using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Itens.Queries;

public sealed record GetItensPagedQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResponse<ItemResponse>>;

public sealed class GetItensPagedQueryHandler(IItemTecnologiaRepository repository)
    : IRequestHandler<GetItensPagedQuery, PagedResponse<ItemResponse>>
{
    public async Task<PagedResponse<ItemResponse>> Handle(
        GetItensPagedQuery request, CancellationToken cancellationToken)
    {
        var itens = await repository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);
        var total = await repository.CountAsync(cancellationToken);

        return new PagedResponse<ItemResponse>(
            itens.Select(i => i.ToResponse()),
            request.Page,
            request.PageSize,
            total);
    }
}
