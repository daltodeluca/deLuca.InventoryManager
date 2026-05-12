using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Features.Subcategorias.Commands;
using deLuca.InventoryManager.Domain.Entities;

namespace deLuca.InventoryManager.Application.Mappings;

public static class SubcategoriaExtensions
{
    public static SubcategoriaResponse ToResponse(this Subcategoria entity)
        => new(entity.Id, entity.Nome, entity.CategoriaId, entity.CategoriaNome ?? string.Empty);

    public static Subcategoria ToEntity(this CreateSubcategoriaCommand command)
        => new() { Nome = command.Nome, CategoriaId = command.CategoriaId };
}
