using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    /// <inheritdoc />
    public partial class addconfigrateproductfkuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateProducts_Users_UserId",
                table: "RateProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_RateProducts_Users",
                table: "RateProducts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateProducts_Users",
                table: "RateProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_RateProducts_Users_UserId",
                table: "RateProducts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
