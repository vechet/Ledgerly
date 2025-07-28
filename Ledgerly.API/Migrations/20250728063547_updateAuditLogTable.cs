using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class updateAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionKeyValue",
                table: "AuditLog");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "AuditLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNo",
                table: "AuditLog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionNo",
                table: "AuditLog");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "AuditLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TransactionKeyValue",
                table: "AuditLog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
