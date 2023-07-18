using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace FourtitudeIntegrated.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContributionPayments",
                columns: table => new
                {
                    ContributionPaymentsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ContributionId = table.Column<int>(type: "int", nullable: false),
                    GeneralLedgerEntry = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionPayments", x => x.ContributionPaymentsId);
                    table.ForeignKey(
                        name: "FK_ContributionPayments_Contributions_ContributionId",
                        column: x => x.ContributionId,
                        principalTable: "Contributions",
                        principalColumn: "ContributionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContributionPayments_GeneralLedger_GeneralLedgerEntry",
                        column: x => x.GeneralLedgerEntry,
                        principalTable: "GeneralLedger",
                        principalColumn: "EntryNo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AmountBorrowed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PenaltyDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PenaltyStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateOfLastPayment = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateDue = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loans_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LoanTransactions",
                columns: table => new
                {
                    LoanTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    GeneralLedgerEntry = table.Column<int>(type: "int", nullable: false),
                    LoanTransactionType = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTransactions", x => x.LoanTransactionId);
                    table.ForeignKey(
                        name: "FK_LoanTransactions_GeneralLedger_GeneralLedgerEntry",
                        column: x => x.GeneralLedgerEntry,
                        principalTable: "GeneralLedger",
                        principalColumn: "EntryNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanTransactions_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "LoanId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionPayments_ContributionId",
                table: "ContributionPayments",
                column: "ContributionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionPayments_GeneralLedgerEntry",
                table: "ContributionPayments",
                column: "GeneralLedgerEntry");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_AccountId",
                table: "Loans",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanTransactions_GeneralLedgerEntry",
                table: "LoanTransactions",
                column: "GeneralLedgerEntry");

            migrationBuilder.CreateIndex(
                name: "IX_LoanTransactions_LoanId",
                table: "LoanTransactions",
                column: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContributionPayments");

            migrationBuilder.DropTable(
                name: "LoanTransactions");

            migrationBuilder.DropTable(
                name: "Loans");
        }
    }
}
