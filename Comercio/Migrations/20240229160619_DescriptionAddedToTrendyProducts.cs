using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comercio.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionAddedToTrendyProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TrendyProducts",
                type: "nvarchar(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TrendyProducts");
        }
    }
}
