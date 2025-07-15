using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    /// <inheritdoc />
    public partial class addconfigrateproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateProducts_Products_ProductId",
                table: "RateProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_RateProducts_Products",
                table: "RateProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateProducts_Products",
                table: "RateProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_RateProducts_Products_ProductId",
                table: "RateProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
