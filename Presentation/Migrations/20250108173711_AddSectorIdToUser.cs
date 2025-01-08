using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tickest.Persistence.Migrations
{
    public partial class AddSectorIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectorId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true, // Permitir valores nulos
                defaultValue: null); // Não usar GUID padrão, pois queremos permitir nulos

            migrationBuilder.CreateIndex(
                name: "IX_Users_SectorId",
                table: "Users",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Sectors_SectorId",
                table: "Users",
                column: "SectorId",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull); // Definindo a ação referencial para SET NULL
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Sectors_SectorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SectorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Users");
        }
    }
}
