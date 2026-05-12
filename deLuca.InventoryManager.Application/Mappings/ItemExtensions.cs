using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Features.Itens.Commands;
using deLuca.InventoryManager.Domain.Entities;

namespace deLuca.InventoryManager.Application.Mappings;

public static class ItemExtensions
{
    public static ItemResponse ToResponse(this ItemTecnologia entity)
        => new(entity.Id, entity.CodigoFormatado, entity.Descricao, entity.SubcategoriaNome ?? string.Empty);

    public static ItemTecnologia ToEntity(this CreateItemCommand command)
        => new() { CodigoFormatado = command.CodigoFormatado, Descricao = command.Descricao, SubcategoriaId = command.SubcategoriaId };
}
