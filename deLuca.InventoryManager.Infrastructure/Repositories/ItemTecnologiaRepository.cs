using Dapper;
using deLuca.InventoryManager.Domain.Entities;
using deLuca.InventoryManager.Domain.Interfaces;
using deLuca.InventoryManager.Infrastructure.Data;

namespace deLuca.InventoryManager.Infrastructure.Repositories;

public sealed class ItemTecnologiaRepository(IDbConnectionFactory connectionFactory) : IItemTecnologiaRepository
{
    public async Task<IEnumerable<ItemTecnologia>> GetPagedAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        const string sql = """
            SELECT i.Id, i.CodigoFormatado, i.Descricao, i.SubcategoriaId, i.Ativo,
                   s.Nome AS SubcategoriaNome
            FROM Itens i
            INNER JOIN Subcategorias s ON s.Id = i.SubcategoriaId
            WHERE i.Ativo = 1 AND s.Ativo = 1
            ORDER BY i.CodigoFormatado
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryAsync<ItemTecnologia>(
            new CommandDefinition(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize }, cancellationToken: ct));
    }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        const string sql = "SELECT COUNT(*) FROM Itens WHERE Ativo = 1";

        using var conn = connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<ItemTecnologia?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT i.Id, i.CodigoFormatado, i.Descricao, i.SubcategoriaId, i.Ativo,
                   s.Nome AS SubcategoriaNome
            FROM Itens i
            INNER JOIN Subcategorias s ON s.Id = i.SubcategoriaId
            WHERE i.Id = @Id AND i.Ativo = 1 AND s.Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<ItemTecnologia>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
    }

    public async Task<int> AddAsync(ItemTecnologia item, CancellationToken ct = default)
    {
        const string sql = """
            INSERT INTO Itens (CodigoFormatado, Descricao, SubcategoriaId, Ativo)
            OUTPUT INSERTED.Id
            VALUES (@CodigoFormatado, @Descricao, @SubcategoriaId, 1)
            """;

        using var conn = connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, new { item.CodigoFormatado, item.Descricao, item.SubcategoriaId }, cancellationToken: ct));
    }

    public async Task<bool> UpdateAsync(ItemTecnologia item, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE Itens
            SET CodigoFormatado = @CodigoFormatado,
                Descricao = @Descricao,
                SubcategoriaId = @SubcategoriaId
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        var rows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { item.CodigoFormatado, item.Descricao, item.SubcategoriaId, item.Id }, cancellationToken: ct));
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE Itens
            SET Ativo = 0
            WHERE Id = @Id AND Ativo = 1
            """;

        using var conn = connectionFactory.CreateConnection();
        var rows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        return rows > 0;
    }
}
