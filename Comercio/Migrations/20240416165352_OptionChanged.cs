using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comercio.Migrations
{
    /// <inheritdoc />
    public partial class OptionChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ProductsOption",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Options",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "OptionGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OptionGroups_CategoryId",
                table: "OptionGroups",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionGroups_Categories_CategoryId",
                table: "OptionGroups",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionGroups_Categories_CategoryId",
                table: "OptionGroups");

            migrationBuilder.DropIndex(
                name: "IX_OptionGroups_CategoryId",
                table: "OptionGroups");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ProductsOption");

            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OptionGroups");
        }
    }
}
