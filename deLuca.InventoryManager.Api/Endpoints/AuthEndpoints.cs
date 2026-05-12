using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Services;

namespace deLuca.InventoryManager.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", (LoginRequest request, IAuthService authService,
            ILoggerFactory loggerFactory, CancellationToken ct) =>
        {
            var token = authService.Authenticate(request, ct);

            if (token is null)
            {
                loggerFactory.CreateLogger("Auth")
                    .LogWarning("Failed login attempt for {Email}", request.Email);
                return Results.Unauthorized();
            }

            return Results.Ok(new { Token = token });
        });

        return app;
    }
}
