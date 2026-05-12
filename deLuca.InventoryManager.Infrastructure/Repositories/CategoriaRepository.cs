using Dapper;
using deLuca.InventoryManager.Domain.Entities;
using deLuca.InventoryManager.Domain.Interfaces;
using deLuca.InventoryManager.Infrastructure.Data;

namespace deLuca.InventoryManager.Infrastructure.Repositories;

public sealed class CategoriaRepository(IDbConnectionFactory connectionFactory) : ICategoriaRepository
{
    public async Task<IEnumerable<Categoria>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT Id, Nome, Prefixo, Ativo
            FROM Categorias
            WHERE Ativo = 1
            ORDER BY Nome
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<Categoria>(new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<Categoria?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT Id, Nome, Prefixo, Ativo
            FROM Categorias
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Categoria>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
    }

    public async Task<int> AddAsync(Categoria categoria, CancellationToken ct = default)
    {
        const string sql = """
            INSERT INTO Categorias (Nome, Prefixo, Ativo)
            OUTPUT INSERTED.Id
            VALUES (@Nome, @Prefixo, 1)
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, new { categoria.Nome, categoria.Prefixo }, cancellationToken: ct));
    }

    public async Task<bool> UpdateAsync(Categoria categoria, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE Categorias
            SET Nome = @Nome, Prefixo = @Prefixo
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        var rows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { categoria.Nome, categoria.Prefixo, categoria.Id }, cancellationToken: ct));
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE Categorias
            SET Ativo = 0
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        var rows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        return rows > 0;
    }
}
