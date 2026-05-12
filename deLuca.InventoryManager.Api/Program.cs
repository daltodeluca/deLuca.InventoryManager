using deLuca.InventoryManager.Api.Extensions;
using deLuca.InventoryManager.Application.Extensions;
using deLuca.InventoryManager.Infrastructure.Data;
using deLuca.InventoryManager.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

await DbInitializer.InitializeAsync(
    app.Services.GetRequiredService<IDbConnectionFactory>());

app.UseApiConfiguration(app.Environment);
app.MapEndpoints();

await app.RunAsync();

public partial class Program { }
