using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations.LedgerlyAuthDb
{
    /// <inheritdoc />
    public partial class initRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a7e160e-fa71-4b01-bdf0-15ef72e7f25f", null, "Admin", "ROLE_ADMIN" },
                    { "c4372eec-9e53-434b-afb6-81083afcd22f", null, "User", "ROLE_USER" },
                    { "fb0890ca-406e-4bf7-ac9e-d4bf979879be", null, "System Admin", "ROLE_SYSTEM_ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a7e160e-fa71-4b01-bdf0-15ef72e7f25f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4372eec-9e53-434b-afb6-81083afcd22f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb0890ca-406e-4bf7-ac9e-d4bf979879be");
        }
    }
}
