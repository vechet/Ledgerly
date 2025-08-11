using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class update8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Status_StatusId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Status_StatusId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_GlobalParam_TransactionFlag",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Status_StatusId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TransactionFlag",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionFlag",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Category",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Account",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Status_StatusId",
                table: "Account",
                column: "StatusId",
                principalTable: "GlobalParam",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_GlobalParam_StatusId",
                table: "Category",
                column: "StatusId",
                principalTable: "GlobalParam",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_GlobalParam_StatusId",
                table: "Transaction",
                column: "StatusId",
                principalTable: "GlobalParam",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Status_StatusId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_GlobalParam_StatusId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_GlobalParam_StatusId",
                table: "Transaction");

            migrationBuilder.AlterColumn<short>(
                name: "StatusId",
                table: "Transaction",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TransactionFlag",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<short>(
                name: "StatusId",
                table: "Category",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "StatusId",
                table: "Account",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "KeyName", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { (short)1, "1", new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", null, null, "Active" },
                    { (short)2, "1", new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive", null, null, "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionFlag",
                table: "Transaction",
                column: "TransactionFlag");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Status_StatusId",
                table: "Account",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Status_StatusId",
                table: "Category",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_GlobalParam_TransactionFlag",
                table: "Transaction",
                column: "TransactionFlag",
                principalTable: "GlobalParam",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Status_StatusId",
                table: "Transaction",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");
        }
    }
}
