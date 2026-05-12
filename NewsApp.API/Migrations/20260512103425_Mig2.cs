using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath2",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath3",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath4",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath5",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath2",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ImagePath3",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ImagePath4",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ImagePath5",
                table: "News");
        }
    }
}
