using deLuca.InventoryManager.Domain.Interfaces;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Subcategorias.Commands;

public sealed record DeleteSubcategoriaCommand(int Id) : IRequest<bool>;

public sealed class DeleteSubcategoriaCommandHandler(ISubcategoriaRepository repository)
    : IRequestHandler<DeleteSubcategoriaCommand, bool>
{
    public async Task<bool> Handle(
        DeleteSubcategoriaCommand request, CancellationToken cancellationToken)
        => await repository.DeleteAsync(request.Id, cancellationToken);
}
