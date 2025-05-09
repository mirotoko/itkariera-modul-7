using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuitarShop.Migrations
{
    /// <inheritdoc />
    public partial class CartAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Purchase",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFullName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Users_UserFullName",
                        column: x => x.UserFullName,
                        principalTable: "Users",
                        principalColumn: "FullName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CartId",
                table: "Purchase",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserFullName",
                table: "Cart",
                column: "UserFullName");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Cart_CartId",
                table: "Purchase",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Cart_CartId",
                table: "Purchase");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Purchase_CartId",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Purchase");
        }
    }
}
