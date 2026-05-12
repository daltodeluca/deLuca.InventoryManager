using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Mappings;
using deLuca.InventoryManager.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Itens.Commands;

public sealed record CreateItemCommand(string CodigoFormatado, string Descricao, int SubcategoriaId)
    : IRequest<ItemResponse>;

public sealed class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.CodigoFormatado)
            .NotEmpty()
            .Matches(@"^[A-Z]{3}-\d{3}$")
            .WithMessage("CodigoFormatado deve seguir o formato AAA-999 (ex: CAB-001).");
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SubcategoriaId).GreaterThan(0);
    }
}

public sealed class CreateItemCommandHandler(IItemTecnologiaRepository repository)
    : IRequestHandler<CreateItemCommand, ItemResponse>
{
    public async Task<ItemResponse> Handle(
        CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = request.ToEntity();
        var id = await repository.AddAsync(item, cancellationToken);
        return new ItemResponse(id, request.CodigoFormatado, request.Descricao, string.Empty);
    }
}
