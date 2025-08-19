using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations.LedgerlyAuthDb
{
    /// <inheritdoc />
    public partial class updateStrucutre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e221a68-5217-4ba1-9716-df7437fb0c70");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96ed14e4-fa2a-4986-9ee0-2d6c387674bd");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-admin-id", "user-id" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin-id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b22ae738-feec-4842-a486-2089afc50f31", null, "ROLE_ADMIN", "ROLE_ADMIN" },
                    { "df2b7de3-4558-4141-9966-137e39bcf6df", null, "ROLE_USER", "ROLE_USER" },
                    { "role-system-admin-id", null, "ROLE_SYSTEM_ADMIN", "ROLE_SYSTEM_ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31490099-3560-4a36-bdab-e91405348994", "AQAAAAIAAYagAAAAEJrFSaQ4FpSIGChx2O45DwJKjSyO9L7vPxb259V9HhV+/N/h8ADtXdp1CDr3kHr1PA==", "bf2913c8-25dc-4f72-a866-8cbf2dcde11e" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "role-system-admin-id", "user-id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b22ae738-feec-4842-a486-2089afc50f31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df2b7de3-4558-4141-9966-137e39bcf6df");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-system-admin-id", "user-id" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-system-admin-id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e221a68-5217-4ba1-9716-df7437fb0c70", null, "ROLE_ADMIN", "ROLE_ADMIN" },
                    { "96ed14e4-fa2a-4986-9ee0-2d6c387674bd", null, "ROLE_USER", "ROLE_USER" },
                    { "role-admin-id", null, "ROLE_SYSTEM_ADMIN", "ROLE_SYSTEM_ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9687137-f14f-469b-9d88-fbb56c128b92", "AQAAAAIAAYagAAAAEGKM5BMN7c+26tXHVAtf5oKWu35Dzakr9lkq+5DrTrzbusfYAinE2Yda3Mfm4xQcAw==", "667f8ab0-5675-4317-8ec6-55d3b5e23dbe" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "role-admin-id", "user-id" });
        }
    }
}
