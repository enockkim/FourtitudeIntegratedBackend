using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourtitudeIntegrated.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedDateToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Transactions",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDateTime",
                table: "GeneralLedger",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EntryDateTime",
                table: "GeneralLedger");
        }
    }
}
