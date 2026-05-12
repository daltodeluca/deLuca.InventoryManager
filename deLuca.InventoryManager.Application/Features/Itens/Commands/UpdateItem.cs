using deLuca.InventoryManager.Domain.Entities;
using deLuca.InventoryManager.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace deLuca.InventoryManager.Application.Features.Itens.Commands;

public sealed record UpdateItemCommand(int Id, string CodigoFormatado, string Descricao, int SubcategoriaId)
    : IRequest<bool>;

public sealed class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.CodigoFormatado)
            .NotEmpty()
            .Matches(@"^[A-Z]{3}-\d{3}$")
            .WithMessage("CodigoFormatado deve seguir o formato AAA-999 (ex: CAB-001).");
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SubcategoriaId).GreaterThan(0);
    }
}

public sealed class UpdateItemCommandHandler(IItemTecnologiaRepository repository)
    : IRequestHandler<UpdateItemCommand, bool>
{
    public async Task<bool> Handle(
        UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new ItemTecnologia
        {
            Id = request.Id,
            CodigoFormatado = request.CodigoFormatado,
            Descricao = request.Descricao,
            SubcategoriaId = request.SubcategoriaId
        };
        return await repository.UpdateAsync(item, cancellationToken);
    }
}
