using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuitarShop.Migrations
{
    /// <inheritdoc />
    public partial class UsersMigrationStringList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guitar_User_UserId",
                table: "Guitar");

            migrationBuilder.DropIndex(
                name: "IX_Guitar_UserId",
                table: "Guitar");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Guitar");

            migrationBuilder.AddColumn<string>(
                name: "Cart",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cart",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Guitar",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guitar_UserId",
                table: "Guitar",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guitar_User_UserId",
                table: "Guitar",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
