using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace deLuca.InventoryManager.Api.Data;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Prefixo { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    [JsonIgnore]
    public ICollection<Subcategoria> Subcategorias { get; set; } = new List<Subcategoria>();
}

public class Subcategoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public bool Ativo { get; set; } = true;
    [JsonIgnore]
    public virtual Categoria? Categoria { get; set; }

    [JsonIgnore]
    public ICollection<ItemTecnologia> Itens { get; set; } = new List<ItemTecnologia>();
}

public class ItemTecnologia
{
    public int Id { get; set; }
    public string CodigoFormatado { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int SubcategoriaId { get; set; }
    public bool Ativo { get; set; } = true;

    [JsonIgnore]
    public virtual Subcategoria? Subcategoria { get; set; }
}

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; } = null!;
    public DbSet<Subcategoria> Subcategorias { get; set; } = null!;
    public DbSet<ItemTecnologia> Itens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nome = "Cabos e Conectividade", Prefixo = "CAB-", Ativo = true },
            new Categoria { Id = 2, Nome = "Periféricos", Prefixo = "PER-", Ativo = true },
            new Categoria { Id = 3, Nome = "Equipamentos / Dispositivos", Prefixo = "EQP-", Ativo = true },
            new Categoria { Id = 4, Nome = "Hardware / Componentes Internos", Prefixo = "HDW-", Ativo = true },
            new Categoria { Id = 5, Nome = "Armazenamento de Dados", Prefixo = "ARM-", Ativo = true },
            new Categoria { Id = 6, Nome = "Ferramentas e Insumos", Prefixo = "FER-", Ativo = true }
        );

        modelBuilder.Entity<Subcategoria>().HasData(
            new Subcategoria { Id = 1, Nome = "Vídeo", CategoriaId = 1, Ativo = true },
            new Subcategoria { Id = 2, Nome = "Dados", CategoriaId = 1, Ativo = true },
            new Subcategoria { Id = 3, Nome = "Rede", CategoriaId = 1, Ativo = true },
            new Subcategoria { Id = 4, Nome = "Áudio", CategoriaId = 1, Ativo = true },
            new Subcategoria { Id = 5, Nome = "Energia", CategoriaId = 1, Ativo = true },

            new Subcategoria { Id = 6, Nome = "Apontamento & Digitação", CategoriaId = 2, Ativo = true },
            new Subcategoria { Id = 7, Nome = "Áudio Visual", CategoriaId = 2, Ativo = true },
            new Subcategoria { Id = 8, Nome = "Exibição", CategoriaId = 2, Ativo = true },
            new Subcategoria { Id = 9, Nome = "Impressão", CategoriaId = 2, Ativo = true },

            new Subcategoria { Id = 10, Nome = "Computação Pessoal", CategoriaId = 3, Ativo = true },
            new Subcategoria { Id = 11, Nome = "Infraestrutura de Rede", CategoriaId = 3, Ativo = true },
            new Subcategoria { Id = 12, Nome = "Energia", CategoriaId = 3, Ativo = true },

            new Subcategoria { Id = 13, Nome = "Processamento", CategoriaId = 4, Ativo = true },
            new Subcategoria { Id = 14, Nome = "Memória", CategoriaId = 4, Ativo = true },
            new Subcategoria { Id = 15, Nome = "Placas", CategoriaId = 4, Ativo = true },
            new Subcategoria { Id = 16, Nome = "Energia Interna", CategoriaId = 4, Ativo = true },
            new Subcategoria { Id = 17, Nome = "Refrigeração", CategoriaId = 4, Ativo = true },

            new Subcategoria { Id = 18, Nome = "Interno", CategoriaId = 5, Ativo = true },
            new Subcategoria { Id = 19, Nome = "Externo", CategoriaId = 5, Ativo = true },
            new Subcategoria { Id = 20, Nome = "Portátil/Flash", CategoriaId = 5, Ativo = true },

            new Subcategoria { Id = 21, Nome = "Ferramentas Manuais", CategoriaId = 6, Ativo = true },
            new Subcategoria { Id = 22, Nome = "Diagnóstico", CategoriaId = 6, Ativo = true },
            new Subcategoria { Id = 23, Nome = "Insumos (Consumíveis)", CategoriaId = 6, Ativo = true }
        );

        modelBuilder.Entity<Categoria>().HasQueryFilter(c => c.Ativo);
        modelBuilder.Entity<Subcategoria>().HasQueryFilter(s => s.Ativo);
        modelBuilder.Entity<ItemTecnologia>().HasQueryFilter(i => i.Ativo);
    }
}
