using FluentValidation;
using deLuca.InventoryManager.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using deLuca.InventoryManager.Api.Data;

namespace deLuca.InventoryManager.Api.Validations;

public class CreateCategoriaValidator : AbstractValidator<CreateCategoriaRequest>
{
    public CreateCategoriaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Prefixo).NotEmpty().Length(4);
    }
}

public class CreateSubcategoriaValidator : AbstractValidator<CreateSubcategoriaRequest>
{
    public CreateSubcategoriaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
    }
}

public class CreateItemValidator : AbstractValidator<CreateItemRequest>
{
    private readonly InventoryContext _db;

    public CreateItemValidator(InventoryContext db)
    {
        _db = db;

        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SubcategoriaId).GreaterThan(0);

        RuleFor(x => x.CodigoFormatado)
            .NotEmpty()
            .Matches("^[A-Z]{3}-\\d{3}$").WithMessage("O Código Formatado deve seguir o formato AAA-999 (Ex: CAB-001)");

        RuleFor(x => x.CodigoFormatado).MustAsync(async (req, codigo, context, ct) =>
        {
            var sub = await _db.Subcategorias.Include(s => s.Categoria).FirstOrDefaultAsync(s => s.Id == req.SubcategoriaId, ct);
            var prefixo = sub?.Categoria?.Prefixo ?? string.Empty;
            var isValid = !string.IsNullOrEmpty(prefixo) && !string.IsNullOrEmpty(codigo) && codigo.StartsWith(prefixo);
            if (!isValid)
            {
                context.AddFailure("CodigoFormatado", $"O código '{codigo}' é inválido para a categoria selecionada. O prefixo correto esperado é '{prefixo}'.");
            }
            return isValid;
        });
    }
}

public class UpdateCategoriaValidator : AbstractValidator<deLuca.InventoryManager.Api.DTOs.UpdateCategoriaRequest>
{
    public UpdateCategoriaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Prefixo).NotEmpty().Length(4);
    }
}

public class UpdateSubcategoriaValidator : AbstractValidator<deLuca.InventoryManager.Api.DTOs.UpdateSubcategoriaRequest>
{
    public UpdateSubcategoriaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
    }
}

public class UpdateItemValidator : AbstractValidator<deLuca.InventoryManager.Api.DTOs.UpdateItemRequest>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SubcategoriaId).GreaterThan(0);
        RuleFor(x => x.CodigoFormatado)
            .NotEmpty()
            .Matches("^[A-Z]{3}-\\d{3}$").WithMessage("O Código Formatado deve seguir o formato AAA-999 (Ex: CAB-001)");
    }
}
