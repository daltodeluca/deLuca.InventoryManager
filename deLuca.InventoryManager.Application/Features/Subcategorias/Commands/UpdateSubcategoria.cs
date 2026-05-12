using deLuca.InventoryManager.Domain.Entities;
using deLuca.InventoryManager.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Subcategorias.Commands;

public sealed record UpdateSubcategoriaCommand(int Id, string Nome, int CategoriaId) : IRequest<bool>;

public sealed class UpdateSubcategoriaCommandValidator : AbstractValidator<UpdateSubcategoriaCommand>
{
    public UpdateSubcategoriaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
    }
}

public sealed class UpdateSubcategoriaCommandHandler(ISubcategoriaRepository repository)
    : IRequestHandler<UpdateSubcategoriaCommand, bool>
{
    public async Task<bool> Handle(
        UpdateSubcategoriaCommand request, CancellationToken cancellationToken)
    {
        var subcategoria = new Subcategoria
        {
            Id = request.Id,
            Nome = request.Nome,
            CategoriaId = request.CategoriaId
        };
        return await repository.UpdateAsync(subcategoria, cancellationToken);
    }
}
