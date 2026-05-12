using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Subcategorias.Commands;

public sealed record CreateSubcategoriaCommand(string Nome, int CategoriaId) : IRequest<SubcategoriaResponse>;

public sealed class CreateSubcategoriaCommandValidator : AbstractValidator<CreateSubcategoriaCommand>
{
    public CreateSubcategoriaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
    }
}

public sealed class CreateSubcategoriaCommandHandler(ISubcategoriaRepository repository)
    : IRequestHandler<CreateSubcategoriaCommand, SubcategoriaResponse>
{
    public async Task<SubcategoriaResponse> Handle(
        CreateSubcategoriaCommand request, CancellationToken cancellationToken)
    {
        var subcategoria = request.ToEntity();
        var id = await repository.AddAsync(subcategoria, cancellationToken);
        return new SubcategoriaResponse(id, request.Nome, request.CategoriaId, string.Empty);
    }
}
