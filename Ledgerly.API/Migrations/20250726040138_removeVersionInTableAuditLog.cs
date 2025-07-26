using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class removeVersionInTableAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "AuditLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "AuditLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
