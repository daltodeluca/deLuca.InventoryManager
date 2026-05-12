using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Itens.Commands;

public sealed record DeleteItemCommand(int Id) : IRequest<bool>;

public sealed class DeleteItemCommandHandler(IItemTecnologiaRepository repository)
    : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(
        DeleteItemCommand request, CancellationToken cancellationToken)
        => await repository.DeleteAsync(request.Id, cancellationToken);
}
