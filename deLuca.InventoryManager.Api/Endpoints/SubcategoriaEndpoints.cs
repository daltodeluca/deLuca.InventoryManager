using Microsoft.EntityFrameworkCore;
using deLuca.InventoryManager.Api.Data;
using deLuca.InventoryManager.Api.DTOs;
using deLuca.InventoryManager.Api.Validations;

public static class SubcategoriaEndpoints
{
    public static void MapSubcategoriaEndpoints(this IEndpointRouteBuilder app)
    {
        var subcategorias = app.MapGroup("/api/subcategorias");

        subcategorias.MapGet("/", async (InventoryContext db) =>
        {
            var list = await db.Subcategorias
                .Select(s => new SubcategoriaResponse(s.Id, s.Nome, s.CategoriaId, s.Categoria != null ? s.Categoria.Nome : string.Empty))
                .ToListAsync();
            return Results.Ok(list);
        });

        subcategorias.MapPost("/", async (CreateSubcategoriaRequest req, InventoryContext db) =>
        {
            var sub = new Subcategoria { Nome = req.Nome, CategoriaId = req.CategoriaId };
            db.Subcategorias.Add(sub);
            await db.SaveChangesAsync();
            var categoria = await db.Categorias.FindAsync(sub.CategoriaId);
            return Results.Created($"/api/subcategorias/{sub.Id}", new SubcategoriaResponse(sub.Id, sub.Nome, sub.CategoriaId, categoria?.Nome ?? string.Empty));
        }).AddEndpointFilter<ValidationFilter<CreateSubcategoriaRequest>>().RequireAuthorization();

        subcategorias.MapPut("/{id}", async (int id, UpdateSubcategoriaRequest req, InventoryContext db) =>
        {
            var sub = await db.Subcategorias.FindAsync(id);
            if (sub is null) return Results.NotFound();
            sub.Nome = req.Nome;
            sub.CategoriaId = req.CategoriaId;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).AddEndpointFilter<ValidationFilter<UpdateSubcategoriaRequest>>().RequireAuthorization();

        subcategorias.MapDelete("/{id}", async (int id, InventoryContext db) =>
        {
            var sub = await db.Subcategorias.FindAsync(id);
            if (sub is null) return Results.NotFound();
            sub.Ativo = false;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).RequireAuthorization();
    }
}