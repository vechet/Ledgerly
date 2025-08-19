using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations.LedgerlyAuthDb
{
    /// <inheritdoc />
    public partial class updateDefaultUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24b365f8-38dd-4872-ae34-36731a098ea7", null, "ROLE_USER", "ROLE_USER" },
                    { "7656e4ef-34d1-4048-b909-2e7f6f34df43", null, "ROLE_ADMIN", "ROLE_ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "system-admin-id", 0, "0543c3b7-c4f2-4614-ab14-947f7ef423de", "systemadmin@gmail.com.com", false, true, null, "SYSTEMADMIN@GMAIL.COM", "SYSTEMADMIN", "AQAAAAIAAYagAAAAEHqDzdnKrcZm+CDj+RtEdDXUxPVcCbHNXXzpbg3fA0wBdt0c5D9ciXu3cBInDgzL5A==", null, false, "9322f5dc-ff49-48b9-abc2-b760ca550109", false, "systemadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "role-system-admin-id", "system-admin-id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24b365f8-38dd-4872-ae34-36731a098ea7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7656e4ef-34d1-4048-b909-2e7f6f34df43");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-system-admin-id", "system-admin-id" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "system-admin-id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b22ae738-feec-4842-a486-2089afc50f31", null, "ROLE_ADMIN", "ROLE_ADMIN" },
                    { "df2b7de3-4558-4141-9966-137e39bcf6df", null, "ROLE_USER", "ROLE_USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "user-id", 0, "31490099-3560-4a36-bdab-e91405348994", "systemadmin@gmail.com.com", false, true, null, "SYSTEMADMIN@GMAIL.COM", "SYSTEMADMIN", "AQAAAAIAAYagAAAAEJrFSaQ4FpSIGChx2O45DwJKjSyO9L7vPxb259V9HhV+/N/h8ADtXdp1CDr3kHr1PA==", null, false, "bf2913c8-25dc-4f72-a866-8cbf2dcde11e", false, "systemadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "role-system-admin-id", "user-id" });
        }
    }
}
