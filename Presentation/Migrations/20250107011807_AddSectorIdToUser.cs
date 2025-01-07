using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tickest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSectorIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectorId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
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
