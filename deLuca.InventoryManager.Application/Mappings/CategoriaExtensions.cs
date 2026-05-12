using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Features.Categorias.Commands;
using deLuca.InventoryManager.Domain.Entities;

namespace deLuca.InventoryManager.Application.Mappings;

public static class CategoriaExtensions
{
    public static CategoriaResponse ToResponse(this Categoria entity)
        => new(entity.Id, entity.Nome, entity.Prefixo);

    public static Categoria ToEntity(this CreateCategoriaCommand command)
        => new() { Nome = command.Nome, Prefixo = command.Prefixo };
}
