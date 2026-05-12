using Dapper;

namespace deLuca.InventoryManager.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IDbConnectionFactory factory, CancellationToken ct = default)
    {
        using var conn = factory.CreateConnection();
        conn.Open();
        await conn.ExecuteAsync(new CommandDefinition(CreateSchema, cancellationToken: ct));
    }

    private const string CreateSchema = """
        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Categorias')
        CREATE TABLE Categorias (
            Id      INT IDENTITY(1,1) PRIMARY KEY,
            Nome    NVARCHAR(100)     NOT NULL,
            Prefixo NVARCHAR(10)      NOT NULL,
            Ativo   BIT               NOT NULL DEFAULT 1
        );

        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Subcategorias')
        CREATE TABLE Subcategorias (
            Id          INT IDENTITY(1,1) PRIMARY KEY,
            Nome        NVARCHAR(100)     NOT NULL,
            CategoriaId INT               NOT NULL,
            Ativo       BIT               NOT NULL DEFAULT 1,
            CONSTRAINT FK_Subcategorias_Categorias
                FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
        );

        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Itens')
        CREATE TABLE Itens (
            Id               INT IDENTITY(1,1) PRIMARY KEY,
            CodigoFormatado  NVARCHAR(50)      NOT NULL,
            Descricao        NVARCHAR(200)     NOT NULL,
            SubcategoriaId   INT               NOT NULL,
            Ativo            BIT               NOT NULL DEFAULT 1,
            CONSTRAINT FK_Itens_Subcategorias
                FOREIGN KEY (SubcategoriaId) REFERENCES Subcategorias(Id)
        );
        """;
}
