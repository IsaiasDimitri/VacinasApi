using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacinasApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vacinas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Fabricante = table.Column<string>(type: "TEXT", nullable: false),
                    Lote = table.Column<string>(type: "TEXT", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    Validade = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PostoId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacinas_Postos_PostoId",
                        column: x => x.PostoId,
                        principalTable: "Postos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Postos_Nome",
                table: "Postos",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacinas_Lote",
                table: "Vacinas",
                column: "Lote",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacinas_PostoId",
                table: "Vacinas",
                column: "PostoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacinas");

            migrationBuilder.DropTable(
                name: "Postos");
        }
    }
}
