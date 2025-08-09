using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledgerly.API.Migrations
{
    /// <inheritdoc />
    public partial class update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "StatusId",
                table: "Transaction",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<int>(
                name: "TransactionFlag",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GlobalParam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalParam", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionFlag",
                table: "Transaction",
                column: "TransactionFlag");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_GlobalParam_TransactionFlag",
                table: "Transaction",
                column: "TransactionFlag",
                principalTable: "GlobalParam",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_GlobalParam_TransactionFlag",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "GlobalParam");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TransactionFlag",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionFlag",
                table: "Transaction");

            migrationBuilder.AlterColumn<short>(
                name: "StatusId",
                table: "Transaction",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);
        }
    }
}
