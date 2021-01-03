using Microsoft.EntityFrameworkCore.Migrations;

namespace Udemy.NetCore5.Angular.Data.Migrations
{
    public partial class UserEntityUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainPhoto",
                table: "Photos",
                newName: "Enabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Enabled",
                table: "Photos",
                newName: "MainPhoto");
        }
    }
}
