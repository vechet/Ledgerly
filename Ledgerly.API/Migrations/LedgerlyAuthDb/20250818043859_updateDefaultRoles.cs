using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations.LedgerlyAuthDb
{
    /// <inheritdoc />
    public partial class updateDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e221a68-5217-4ba1-9716-df7437fb0c70", null, "ROLE_ADMIN", "ROLE_ADMIN" },
                    { "96ed14e4-fa2a-4986-9ee0-2d6c387674bd", null, "ROLE_USER", "ROLE_USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9687137-f14f-469b-9d88-fbb56c128b92", "AQAAAAIAAYagAAAAEGKM5BMN7c+26tXHVAtf5oKWu35Dzakr9lkq+5DrTrzbusfYAinE2Yda3Mfm4xQcAw==", "667f8ab0-5675-4317-8ec6-55d3b5e23dbe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e221a68-5217-4ba1-9716-df7437fb0c70");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96ed14e4-fa2a-4986-9ee0-2d6c387674bd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a5e2b77-1269-45f9-b12a-ab39427cc980", "AQAAAAIAAYagAAAAEDTOSNMv6bFRZsShlLh+nwvBBfFCRoctHoZQdV7gkrEeJ8FSJwzp72p4CuLuRIKExQ==", "9b9d2a5b-85c3-4467-81ed-d0e9b823dfea" });
        }
    }
}
