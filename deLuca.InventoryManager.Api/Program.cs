using Microsoft.EntityFrameworkCore;
using deLuca.InventoryManager.Api.Data;
using deLuca.InventoryManager.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using deLuca.InventoryManager.Api.Validations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inserir somente o token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateItemValidator>();
builder.Services.AddProblemDetails();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException("JWT Key is not configured or is too short. Set 'Jwt:Key' in configuration with at least 32 characters.");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey
        };
    });

builder.Services.AddAuthorization();

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var adminEmail = builder.Configuration["AdminConfig:Email"];
var adminPassword = builder.Configuration["AdminConfig:Password"];

builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseNpgsql(defaultConnection, npgsqlOptions =>
        npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null)
    )
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();

var auth = app.MapGroup("/api/auth");
auth.MapPost("/login", (LoginRequest req) =>
{
    var logger = app.Logger;

    if (req.Email == adminEmail && req.Password == adminPassword)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, req.Email), new Claim(ClaimTypes.Role, "Admin") };
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { Token = tokenString });
    }

    logger.LogWarning("Failed login attempt for {Email}", req.Email);
    return Results.Unauthorized();
});

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
    var resp = new CategoriaResponse(categoria.Id, categoria.Nome, categoria.Prefixo);
    return Results.Created($"/api/categorias/{categoria.Id}", resp);
}).AddEndpointFilter<deLuca.InventoryManager.Api.Validations.ValidationFilter<CreateCategoriaRequest>>().RequireAuthorization();

categorias.MapPut("/{id}", async (int id, deLuca.InventoryManager.Api.DTOs.UpdateCategoriaRequest req, InventoryContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null) return Results.NotFound();
    categoria.Nome = req.Nome;
    categoria.Prefixo = req.Prefixo;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).AddEndpointFilter<deLuca.InventoryManager.Api.Validations.ValidationFilter<deLuca.InventoryManager.Api.DTOs.UpdateCategoriaRequest>>().RequireAuthorization();

var subcategorias = app.MapGroup("/api/subcategorias");

subcategorias.MapGet("/", async (InventoryContext db) =>
{
    var list = await db.Subcategorias
        .Select(s => new SubcategoriaResponse(s.Id, s.Nome, s.Categoria != null ? s.Categoria.Nome : string.Empty))
        .ToListAsync();
    return Results.Ok(list);
});

subcategorias.MapPost("/", async (CreateSubcategoriaRequest req, InventoryContext db) =>
{
    var sub = new Subcategoria { Nome = req.Nome, CategoriaId = req.CategoriaId };
    db.Subcategorias.Add(sub);
    await db.SaveChangesAsync();
    var categoria = await db.Categorias.FindAsync(sub.CategoriaId);
    var resp = new SubcategoriaResponse(sub.Id, sub.Nome, categoria?.Nome ?? string.Empty);
    return Results.Created($"/api/subcategorias/{sub.Id}", resp);
}).AddEndpointFilter<deLuca.InventoryManager.Api.Validations.ValidationFilter<CreateSubcategoriaRequest>>().RequireAuthorization();

subcategorias.MapPut("/{id}", async (int id, deLuca.InventoryManager.Api.DTOs.UpdateSubcategoriaRequest req, InventoryContext db) =>
{
    var sub = await db.Subcategorias.FindAsync(id);
    if (sub is null) return Results.NotFound();
    sub.Nome = req.Nome;
    sub.CategoriaId = req.CategoriaId;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).AddEndpointFilter<deLuca.InventoryManager.Api.Validations.ValidationFilter<deLuca.InventoryManager.Api.DTOs.UpdateSubcategoriaRequest>>().RequireAuthorization();

var itens = app.MapGroup("/api/itens");

itens.MapGet("/", async (InventoryContext db, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
{
    var totalItems = await db.Itens.CountAsync();

    var query = db.Itens
        .Select(i => new ItemResponse(i.Id, i.CodigoFormatado, i.Descricao, i.Subcategoria != null ? i.Subcategoria.Nome : string.Empty))
        .Skip((page - 1) * pageSize)
        .Take(pageSize);

    var list = await query.ToListAsync();

    var response = new PagedResponse<ItemResponse>
    {
        Data = list,
        CurrentPage = page,
        PageSize = pageSize,
        TotalItems = totalItems
    };

    return Results.Ok(response);
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
    var resp = new ItemResponse(item.Id, item.CodigoFormatado, item.Descricao, sub?.Nome ?? string.Empty);
    return Results.Created($"/api/itens/{item.Id}", resp);
}).AddEndpointFilter<deLuca.InventoryManager.Api.Validations.ValidationFilter<CreateItemRequest>>().RequireAuthorization();

itens.MapPut("/{id}", async (int id, deLuca.InventoryManager.Api.DTOs.UpdateItemRequest req, InventoryContext db) =>
{
    var item = await db.Itens.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.CodigoFormatado = req.CodigoFormatado;
    item.Descricao = req.Descricao;
    item.SubcategoriaId = req.SubcategoriaId;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).AddEndpointFilter<deLuca.InventoryManager.Api.Validations.ValidationFilter<deLuca.InventoryManager.Api.DTOs.UpdateItemRequest>>().RequireAuthorization();

itens.MapDelete("/{id}", async (int id, InventoryContext db) =>
{
    var item = await db.Itens.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Ativo = false;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

subcategorias.MapDelete("/{id}", async (int id, InventoryContext db) =>
{
    var sub = await db.Subcategorias.FindAsync(id);
    if (sub is null) return Results.NotFound();
    sub.Ativo = false;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

categorias.MapDelete("/{id}", async (int id, InventoryContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null) return Results.NotFound();
    categoria.Ativo = false;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.Run();