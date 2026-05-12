using Dapper;
using deLuca.InventoryManager.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace deLuca.InventoryManager.IntegrationTests.Fixtures;

public sealed class IntegrationTestFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    private WebApplicationFactory<Program>? _factory;

    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IDbConnectionFactory>();
                    services.AddSingleton<IDbConnectionFactory>(
                        new SqlConnectionFactory(_container.GetConnectionString()));
                });
            });

        Client = _factory.CreateClient();
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = _factory!.Services.CreateScope();
        var connFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var conn = connFactory.CreateConnection();
        conn.Open();

        await conn.ExecuteAsync("DELETE FROM Itens");
        await conn.ExecuteAsync("DELETE FROM Subcategorias");
        await conn.ExecuteAsync("DELETE FROM Categorias");
        await conn.ExecuteAsync("DBCC CHECKIDENT ('Categorias', RESEED, 0)");
        await conn.ExecuteAsync("DBCC CHECKIDENT ('Subcategorias', RESEED, 0)");
        await conn.ExecuteAsync("DBCC CHECKIDENT ('Itens', RESEED, 0)");
    }

    public async Task<string> GetAdminTokenAsync()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Email = "admin@deluca.com", Password = "admin123" });

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return body!.Token;
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        if (_factory is not null) await _factory.DisposeAsync();
        await _container.DisposeAsync();
    }

    private sealed record TokenResponse(string Token);
}
