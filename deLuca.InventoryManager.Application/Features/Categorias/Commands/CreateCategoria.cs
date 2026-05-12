using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Categorias.Commands;

public sealed record CreateCategoriaCommand(string Nome, string Prefixo) : IRequest<CategoriaResponse>;

public sealed class CreateCategoriaCommandValidator : AbstractValidator<CreateCategoriaCommand>
{
    public CreateCategoriaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Prefixo).NotEmpty().Length(4)
            .WithMessage("Prefixo deve ter exatamente 4 caracteres.");
    }
}

public sealed class CreateCategoriaCommandHandler(ICategoriaRepository repository)
    : IRequestHandler<CreateCategoriaCommand, CategoriaResponse>
{
    public async Task<CategoriaResponse> Handle(
        CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = request.ToEntity();
        var id = await repository.AddAsync(categoria, cancellationToken);
        return new CategoriaResponse(id, request.Nome, request.Prefixo);
    }
}
