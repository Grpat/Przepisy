using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hostele.Migrations
{
    /// <inheritdoc />
    public partial class PrzepisyV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "RecipeIngredients",
                type: "decimal(5,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,1)");

            migrationBuilder.AddColumn<int>(
                name: "MeasureNumber",
                table: "RecipeIngredients",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasureNumber",
                table: "RecipeIngredients");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "RecipeIngredients",
                type: "decimal(5,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,1)",
                oldNullable: true);
        }
    }
}
