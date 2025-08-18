using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class updateDefaultCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsSystemValue",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsSystemValue",
                value: false);
        }
    }
}
