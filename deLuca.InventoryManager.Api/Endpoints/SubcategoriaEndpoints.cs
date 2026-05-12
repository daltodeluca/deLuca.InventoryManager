using deLuca.InventoryManager.Api.Filters;
using deLuca.InventoryManager.Application.Features.Subcategorias.Commands;
using deLuca.InventoryManager.Application.Features.Subcategorias.Queries;
using MediatR;

namespace deLuca.InventoryManager.Api.Endpoints;

public static class SubcategoriaEndpoints
{
    public static IEndpointRouteBuilder MapSubcategoriaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/subcategorias").WithTags("Subcategorias");

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetAllSubcategoriasQuery(), ct)));

        group.MapGet("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetSubcategoriaByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPost("/", async (CreateSubcategoriaCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/subcategorias/{result.Id}", result);
        })
        .AddEndpointFilter<ValidationFilter<CreateSubcategoriaCommand>>()
        .RequireAuthorization();

        group.MapPut("/{id:int}", async (int id, UpdateSubcategoriaCommand command, ISender sender, CancellationToken ct) =>
        {
            var updated = await sender.Send(command with { Id = id }, ct);
            return updated ? Results.NoContent() : Results.NotFound();
        })
        .AddEndpointFilter<ValidationFilter<UpdateSubcategoriaCommand>>()
        .RequireAuthorization();

        group.MapDelete("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var deleted = await sender.Send(new DeleteSubcategoriaCommand(id), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .RequireAuthorization();

        return app;
    }
}
