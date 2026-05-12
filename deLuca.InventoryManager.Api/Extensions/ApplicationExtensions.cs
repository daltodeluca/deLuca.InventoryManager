using deLuca.InventoryManager.Api.Endpoints;

namespace deLuca.InventoryManager.Api.Extensions;

public static class ApplicationExtensions
{
    public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors("AllowAngular");
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapCategoriaEndpoints();
        app.MapSubcategoriaEndpoints();
        app.MapItemEndpoints();
    }
}
