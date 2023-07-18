using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourtitudeIntegrated.Migrations
{
    /// <inheritdoc />
    public partial class AddedLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PenaltyWaived",
                table: "Contributions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyWaived",
                table: "Contributions");
        }
    }
}
