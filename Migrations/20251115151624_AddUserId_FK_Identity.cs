using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId_FK_Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evolucoes_Usuarios_UsuarioId",
                table: "Evolucoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_Usuarios_UsuarioId",
                table: "Treinos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Treinos_UsuarioId",
                table: "Treinos");

            migrationBuilder.DropIndex(
                name: "IX_Evolucoes_UsuarioId",
                table: "Evolucoes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Evolucoes");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Treinos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exercicios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Evolucoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercicios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Evolucoes");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Treinos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Evolucoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_UsuarioId",
                table: "Treinos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolucoes_UsuarioId",
                table: "Evolucoes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evolucoes_Usuarios_UsuarioId",
                table: "Evolucoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_Usuarios_UsuarioId",
                table: "Treinos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
