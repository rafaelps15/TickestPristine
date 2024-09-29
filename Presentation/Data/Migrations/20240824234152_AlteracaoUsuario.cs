using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tickest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "TB_AREA",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "TB_USUARIO",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "TB_USUARIO");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TB_AREA",
                newName: "Nome");
        }
    }
}
