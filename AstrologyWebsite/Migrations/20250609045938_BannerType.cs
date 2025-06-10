using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrologyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class BannerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bannerType",
                table: "Banners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bannerType",
                table: "Banners");
        }
    }
}
