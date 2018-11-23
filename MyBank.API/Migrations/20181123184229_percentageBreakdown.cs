using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBank.API.Migrations
{
    public partial class percentageBreakdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionDetail",
                table: "TransactionHistories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PercentageBreakdownID",
                table: "Accounts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PercentageBreakdowns",
                columns: table => new
                {
                    PercentageBreakdownID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PercentageBreakdownName = table.Column<string>(nullable: true),
                    PercentageAmount = table.Column<decimal>(nullable: false),
                    PercentageTotal = table.Column<decimal>(nullable: false),
                    accountID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentageBreakdowns", x => x.PercentageBreakdownID);
                    table.ForeignKey(
                        name: "FK_PercentageBreakdowns_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PercentageBreakdownID",
                table: "Accounts",
                column: "PercentageBreakdownID");

            migrationBuilder.CreateIndex(
                name: "IX_PercentageBreakdowns_accountID",
                table: "PercentageBreakdowns",
                column: "accountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_PercentageBreakdowns_PercentageBreakdownID",
                table: "Accounts",
                column: "PercentageBreakdownID",
                principalTable: "PercentageBreakdowns",
                principalColumn: "PercentageBreakdownID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_PercentageBreakdowns_PercentageBreakdownID",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "PercentageBreakdowns");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PercentageBreakdownID",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TransactionDetail",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "PercentageBreakdownID",
                table: "Accounts");
        }
    }
}
