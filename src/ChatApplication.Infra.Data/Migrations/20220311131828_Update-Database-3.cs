using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApplication.Infra.Data.Migrations
{
    public partial class UpdateDatabase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "JoinRequests",
                newName: "JoinRequests",
                newSchema: "Chat");

            migrationBuilder.RenameTable(
                name: "ConnectionRequests",
                newName: "ConnectionRequests",
                newSchema: "Chat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "JoinRequests",
                schema: "Chat",
                newName: "JoinRequests");

            migrationBuilder.RenameTable(
                name: "ConnectionRequests",
                schema: "Chat",
                newName: "ConnectionRequests");
        }
    }
}
