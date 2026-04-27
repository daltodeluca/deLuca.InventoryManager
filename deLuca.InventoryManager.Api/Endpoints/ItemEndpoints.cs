using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using deLuca.InventoryManager.Api.Data;
using deLuca.InventoryManager.Api.DTOs;
using deLuca.InventoryManager.Api.Validations;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this IEndpointRouteBuilder app)
    {
        var itens = app.MapGroup("/api/itens");

        itens.MapGet("/", async (InventoryContext db, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
        {
            var totalItems = await db.Itens.CountAsync();
            var query = db.Itens
                .Select(i => new ItemResponse(i.Id, i.CodigoFormatado, i.Descricao, i.Subcategoria != null ? i.Subcategoria.Nome : string.Empty))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return Results.Ok(new PagedResponse<ItemResponse>
            {
                Data = await query.ToListAsync(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            });
        });

        itens.MapGet("/{id}", async (int id, InventoryContext db) =>
        {
            var item = await db.Itens
                .Where(i => i.Id == id)
                .Select(i => new ItemResponse(i.Id, i.CodigoFormatado, i.Descricao, i.Subcategoria != null ? i.Subcategoria.Nome : string.Empty))
                .FirstOrDefaultAsync();
            return item is not null ? Results.Ok(item) : Results.NotFound();
        });

        itens.MapPost("/", async (CreateItemRequest req, InventoryContext db) =>
        {
            var item = new ItemTecnologia { CodigoFormatado = req.CodigoFormatado, Descricao = req.Descricao, SubcategoriaId = req.SubcategoriaId };
            db.Itens.Add(item);
            await db.SaveChangesAsync();
            var sub = await db.Subcategorias.FindAsync(item.SubcategoriaId);
            return Results.Created($"/api/itens/{item.Id}", new ItemResponse(item.Id, item.CodigoFormatado, item.Descricao, sub?.Nome ?? string.Empty));
        }).AddEndpointFilter<ValidationFilter<CreateItemRequest>>().RequireAuthorization();

        itens.MapPut("/{id}", async (int id, UpdateItemRequest req, InventoryContext db) =>
        {
            var item = await db.Itens.FindAsync(id);
            if (item is null) return Results.NotFound();
            item.CodigoFormatado = req.CodigoFormatado;
            item.Descricao = req.Descricao;
            item.SubcategoriaId = req.SubcategoriaId;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).AddEndpointFilter<ValidationFilter<UpdateItemRequest>>().RequireAuthorization();

        itens.MapDelete("/{id}", async (int id, InventoryContext db) =>
        {
            var item = await db.Itens.FindAsync(id);
            if (item is null) return Results.NotFound();
            item.Ativo = false;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).RequireAuthorization();
    }
}