using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArabaKiralama.Migrations
{
    /// <inheritdoc />
    public partial class FotoAlanEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cars",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cars");

            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
