using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledgerly.API.Migrations.LedgerlyAuthDb
{
    /// <inheritdoc />
    public partial class updateDefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a5e2b77-1269-45f9-b12a-ab39427cc980", true, "AQAAAAIAAYagAAAAEDTOSNMv6bFRZsShlLh+nwvBBfFCRoctHoZQdV7gkrEeJ8FSJwzp72p4CuLuRIKExQ==", "9b9d2a5b-85c3-4467-81ed-d0e9b823dfea" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75b25c7b-0505-4639-a976-2f1329bb74d8", false, "AQAAAAIAAYagAAAAENSDEYj73eUe8HpsE2o/qhHIhSzBMD05XeOO38bjLXV0kKSPweGtqqzkhosfw9obfQ==", "ff317cc4-d646-4990-820f-464905443c4b" });
        }
    }
}
