using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class initDataForCategoryAndGlobalParam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GlobalParam",
                columns: new[] { "Id", "KeyName", "Memo", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Normal", null, "Normal", "CategoryxxxStatus" },
                    { 2, "Deleted", null, "Deleted", "CategoryxxxStatus" },
                    { 3, "Normal", null, "Normal", "TransactionxxxStatus" },
                    { 4, "Deleted", null, "Deleted", "TransactionxxxStatus" },
                    { 5, "Normal", null, "Normal", "AccountxxxStatus" },
                    { 6, "Deleted", null, "Deleted", "AccountxxxStatus" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IconName", "IsSystemValue", "Memo", "ModifiedBy", "ModifiedDate", "Name", "ParentId", "StatusId", "UserId" },
                values: new object[] { 1, "1", new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, null, "Main", null, 1, "user-id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GlobalParam",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GlobalParam",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GlobalParam",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GlobalParam",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GlobalParam",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GlobalParam",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
