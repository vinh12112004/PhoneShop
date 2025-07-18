using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    /// <inheritdoc />
    public partial class refactorImageToListImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ImageUrls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrls",
                table: "Products",
                newName: "ImageUrl");
        }
    }
}
