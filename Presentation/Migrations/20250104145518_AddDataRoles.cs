using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tickest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDataRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" , "CreatedAt" },
                values: new object[,]
                {
                    {Guid.NewGuid(), "AdminMaster", DateTime.Now},
                    {Guid.NewGuid(), "GeneralAdmin", DateTime.Now },
                    {Guid.NewGuid(), "SectorAdmin", DateTime.Now },
                    {Guid.NewGuid(), "AreaAdmin", DateTime.Now },
                    {Guid.NewGuid(), "TicketManager", DateTime.Now },
                    {Guid.NewGuid(), "Collaborator", DateTime.Now },
                    {Guid.NewGuid(), "SupportAnalyst", DateTime.Now }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Roles");
        }
    }
}
