using deLuca.InventoryManager.Api.Filters;
using deLuca.InventoryManager.Application.Features.Itens.Commands;
using deLuca.InventoryManager.Application.Features.Itens.Queries;
using MediatR;

namespace deLuca.InventoryManager.Api.Endpoints;

public static class ItemEndpoints
{
    public static IEndpointRouteBuilder MapItemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/itens").WithTags("Itens");

        group.MapGet("/", async (ISender sender, CancellationToken ct,
            int page = 1, int pageSize = 10) =>
            Results.Ok(await sender.Send(new GetItensPagedQuery(page, pageSize), ct)));

        // Rota específica para detalhes do item: /api/itens/detalhes/{id}
        group.MapGet("/detalhes/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetItemByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPost("/", async (CreateItemCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/itens/detalhes/{result.Id}", result);
        })
        .AddEndpointFilter<ValidationFilter<CreateItemCommand>>()
        .RequireAuthorization();

        group.MapPut("/{id:int}", async (int id, UpdateItemCommand command, ISender sender, CancellationToken ct) =>
        {
            var updated = await sender.Send(command with { Id = id }, ct);
            return updated ? Results.NoContent() : Results.NotFound();
        })
        .AddEndpointFilter<ValidationFilter<UpdateItemCommand>>()
        .RequireAuthorization();

        group.MapDelete("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var deleted = await sender.Send(new DeleteItemCommand(id), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .RequireAuthorization();

        return app;
    }
}
