using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrologyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionToZodiac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Zodiacs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Zodiacs");
        }
    }
}
