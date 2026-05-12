using deLuca.InventoryManager.Api.Filters;
using deLuca.InventoryManager.Application.Features.Categorias.Commands;
using deLuca.InventoryManager.Application.Features.Categorias.Queries;
using MediatR;

namespace deLuca.InventoryManager.Api.Endpoints;

public static class CategoriaEndpoints
{
    public static IEndpointRouteBuilder MapCategoriaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categorias").WithTags("Categorias");

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetAllCategoriasQuery(), ct)));

        group.MapGet("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCategoriaByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPost("/", async (CreateCategoriaCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/categorias/{result.Id}", result);
        })
        .AddEndpointFilter<ValidationFilter<CreateCategoriaCommand>>()
        .RequireAuthorization();

        group.MapPut("/{id:int}", async (int id, UpdateCategoriaCommand command, ISender sender, CancellationToken ct) =>
        {
            var updated = await sender.Send(command with { Id = id }, ct);
            return updated ? Results.NoContent() : Results.NotFound();
        })
        .AddEndpointFilter<ValidationFilter<UpdateCategoriaCommand>>()
        .RequireAuthorization();

        group.MapDelete("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var deleted = await sender.Send(new DeleteCategoriaCommand(id), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .RequireAuthorization();

        return app;
    }
}
