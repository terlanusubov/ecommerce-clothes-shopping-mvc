using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comercio.Migrations
{
    /// <inheritdoc />
    public partial class OptionGroupChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionGroupId",
                table: "Options",
                column: "OptionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupId",
                table: "Options",
                column: "OptionGroupId",
                principalTable: "OptionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionGroupId",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Options_OptionGroupId",
                table: "Options");
        }
    }
}
