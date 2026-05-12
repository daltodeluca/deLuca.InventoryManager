using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Categorias.Commands;

public sealed record DeleteCategoriaCommand(int Id) : IRequest<bool>;

public sealed class DeleteCategoriaCommandHandler(ICategoriaRepository repository)
    : IRequestHandler<DeleteCategoriaCommand, bool>
{
    public async Task<bool> Handle(
        DeleteCategoriaCommand request, CancellationToken cancellationToken)
        => await repository.DeleteAsync(request.Id, cancellationToken);
}
