using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBank.API.Migrations
{
    public partial class initialcommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    passwordHash = table.Column<byte[]>(nullable: true),
                    passwordSalt = table.Column<byte[]>(nullable: true),
                    emailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    accountID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountName = table.Column<string>(nullable: true),
                    accountTotal = table.Column<decimal>(nullable: false),
                    accountPercent = table.Column<decimal>(nullable: false),
                    userID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.accountID);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "TransactionHistories",
                columns: table => new
                {
                    transactionHistoryID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    transactionDateTime = table.Column<DateTime>(nullable: false),
                    transactionType = table.Column<string>(nullable: true),
                    TransactionDetail = table.Column<string>(nullable: true),
                    accountID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistories", x => x.transactionHistoryID);
                    table.ForeignKey(
                        name: "FK_TransactionHistories_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_userID",
                table: "Accounts",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_PercentageBreakdowns_accountID",
                table: "PercentageBreakdowns",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_accountID",
                table: "TransactionHistories",
                column: "accountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PercentageBreakdowns");

            migrationBuilder.DropTable(
                name: "TransactionHistories");

            migrationBuilder.DropTable(
                name: "Values");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
