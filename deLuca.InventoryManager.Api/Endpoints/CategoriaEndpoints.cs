using Microsoft.EntityFrameworkCore;
using deLuca.InventoryManager.Api.Data;
using deLuca.InventoryManager.Api.DTOs;
using deLuca.InventoryManager.Api.Validations;

public static class CategoriaEndpoints
{
    public static void MapCategoriaEndpoints(this IEndpointRouteBuilder app)
    {
        var categorias = app.MapGroup("/api/categorias");

        categorias.MapGet("/", async (InventoryContext db) =>
        {
            var list = await db.Categorias
                .Select(c => new CategoriaResponse(c.Id, c.Nome, c.Prefixo))
                .ToListAsync();
            return Results.Ok(list);
        });

        categorias.MapPost("/", async (CreateCategoriaRequest req, InventoryContext db) =>
        {
            var categoria = new Categoria { Nome = req.Nome, Prefixo = req.Prefixo };
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();
            return Results.Created($"/api/categorias/{categoria.Id}", new CategoriaResponse(categoria.Id, categoria.Nome, categoria.Prefixo));
        }).AddEndpointFilter<ValidationFilter<CreateCategoriaRequest>>().RequireAuthorization();

        categorias.MapPut("/{id}", async (int id, UpdateCategoriaRequest req, InventoryContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);
            if (categoria is null) return Results.NotFound();
            categoria.Nome = req.Nome;
            categoria.Prefixo = req.Prefixo;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).AddEndpointFilter<ValidationFilter<UpdateCategoriaRequest>>().RequireAuthorization();

        categorias.MapDelete("/{id}", async (int id, InventoryContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);
            if (categoria is null) return Results.NotFound();
            categoria.Ativo = false;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).RequireAuthorization();
    }
}