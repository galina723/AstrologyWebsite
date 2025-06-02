using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrologyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarToBlogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Blogs");
        }
    }
}
