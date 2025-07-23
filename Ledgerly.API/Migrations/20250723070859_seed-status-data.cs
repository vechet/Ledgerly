using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class seedstatusdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "KeyName", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { (short)1, "1", new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", null, null, "Active" },
                    { (short)2, "1", new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive", null, null, "Inactive" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: (short)2);
        }
    }
}
