using Dapper;
using deLuca.InventoryManager.Domain.Entities;
using deLuca.InventoryManager.Domain.Interfaces;
using deLuca.InventoryManager.Infrastructure.Data;

namespace deLuca.InventoryManager.Infrastructure.Repositories;

public sealed class SubcategoriaRepository(IDbConnectionFactory connectionFactory) : ISubcategoriaRepository
{
    public async Task<IEnumerable<Subcategoria>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT s.Id, s.Nome, s.CategoriaId, s.Ativo, c.Nome AS CategoriaNome
            FROM Subcategorias s
            INNER JOIN Categorias c ON c.Id = s.CategoriaId
            WHERE s.Ativo = 1 AND c.Ativo = 1
            ORDER BY c.Nome, s.Nome
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<Subcategoria>(new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<Subcategoria?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT s.Id, s.Nome, s.CategoriaId, s.Ativo, c.Nome AS CategoriaNome
            FROM Subcategorias s
            INNER JOIN Categorias c ON c.Id = s.CategoriaId
            WHERE s.Id = @Id AND s.Ativo = 1 AND c.Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Subcategoria>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
    }

    public async Task<int> AddAsync(Subcategoria subcategoria, CancellationToken ct = default)
    {
        const string sql = """
            INSERT INTO Subcategorias (Nome, CategoriaId, Ativo)
            OUTPUT INSERTED.Id
            VALUES (@Nome, @CategoriaId, 1)
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, new { subcategoria.Nome, subcategoria.CategoriaId }, cancellationToken: ct));
    }

    public async Task<bool> UpdateAsync(Subcategoria subcategoria, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE Subcategorias
            SET Nome = @Nome, CategoriaId = @CategoriaId
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        var rows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { subcategoria.Nome, subcategoria.CategoriaId, subcategoria.Id }, cancellationToken: ct));
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE Subcategorias
            SET Ativo = 0
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        var rows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        return rows > 0;
    }
}
