using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace deLuca.InventoryManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Prefixo = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoFormatado = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    SubcategoriaId = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itens_Subcategorias_SubcategoriaId",
                        column: x => x.SubcategoriaId,
                        principalTable: "Subcategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativo", "Nome", "Prefixo" },
                values: new object[,]
                {
                    { 1, true, "Cabos e Conectividade", "CAB-" },
                    { 2, true, "Periféricos", "PER-" },
                    { 3, true, "Equipamentos / Dispositivos", "EQP-" },
                    { 4, true, "Hardware / Componentes Internos", "HDW-" },
                    { 5, true, "Armazenamento de Dados", "ARM-" },
                    { 6, true, "Ferramentas e Insumos", "FER-" }
                });

            migrationBuilder.InsertData(
                table: "Subcategorias",
                columns: new[] { "Id", "Ativo", "CategoriaId", "Nome" },
                values: new object[,]
                {
                    { 1, true, 1, "Vídeo" },
                    { 2, true, 1, "Dados" },
                    { 3, true, 1, "Rede" },
                    { 4, true, 1, "Áudio" },
                    { 5, true, 1, "Energia" },
                    { 6, true, 2, "Apontamento & Digitação" },
                    { 7, true, 2, "Áudio Visual" },
                    { 8, true, 2, "Exibição" },
                    { 9, true, 2, "Impressão" },
                    { 10, true, 3, "Computação Pessoal" },
                    { 11, true, 3, "Infraestrutura de Rede" },
                    { 12, true, 3, "Energia" },
                    { 13, true, 4, "Processamento" },
                    { 14, true, 4, "Memória" },
                    { 15, true, 4, "Placas" },
                    { 16, true, 4, "Energia Interna" },
                    { 17, true, 4, "Refrigeração" },
                    { 18, true, 5, "Interno" },
                    { 19, true, 5, "Externo" },
                    { 20, true, 5, "Portátil/Flash" },
                    { 21, true, 6, "Ferramentas Manuais" },
                    { 22, true, 6, "Diagnóstico" },
                    { 23, true, 6, "Insumos (Consumíveis)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itens_SubcategoriaId",
                table: "Itens",
                column: "SubcategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategorias_CategoriaId",
                table: "Subcategorias",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "Subcategorias");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
