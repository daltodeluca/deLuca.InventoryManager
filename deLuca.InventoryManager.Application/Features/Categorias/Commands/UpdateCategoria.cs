using deLuca.InventoryManager.Domain.Entities;
using deLuca.InventoryManager.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Categorias.Commands;

public sealed record UpdateCategoriaCommand(int Id, string Nome, string Prefixo) : IRequest<bool>;

public sealed class UpdateCategoriaCommandValidator : AbstractValidator<UpdateCategoriaCommand>
{
    public UpdateCategoriaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Prefixo).NotEmpty().Length(4)
            .WithMessage("Prefixo deve ter exatamente 4 caracteres.");
    }
}

public sealed class UpdateCategoriaCommandHandler(ICategoriaRepository repository)
    : IRequestHandler<UpdateCategoriaCommand, bool>
{
    public async Task<bool> Handle(
        UpdateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = new Categoria
        {
            Id = request.Id,
            Nome = request.Nome,
            Prefixo = request.Prefixo
        };
        return await repository.UpdateAsync(categoria, cancellationToken);
    }
}
