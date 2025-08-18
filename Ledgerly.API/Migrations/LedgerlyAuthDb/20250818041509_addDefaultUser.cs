using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations.LedgerlyAuthDb
{
    /// <inheritdoc />
    public partial class addDefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "role-admin-id", null, "ROLE_SYSTEM_ADMIN", "ROLE_SYSTEM_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "user-id", 0, "75b25c7b-0505-4639-a976-2f1329bb74d8", "systemadmin@gmail.com.com", false, false, null, "SYSTEMADMIN@GMAIL.COM", "SYSTEMADMIN", "AQAAAAIAAYagAAAAENSDEYj73eUe8HpsE2o/qhHIhSzBMD05XeOO38bjLXV0kKSPweGtqqzkhosfw9obfQ==", null, false, "ff317cc4-d646-4990-820f-464905443c4b", false, "systemadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "role-admin-id", "user-id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-admin-id", "user-id" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin-id");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id");

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
    }
}
