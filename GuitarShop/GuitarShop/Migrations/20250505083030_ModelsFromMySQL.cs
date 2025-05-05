using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuitarShop.Migrations
{
    /// <inheritdoc />
    public partial class ModelsFromMySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Courier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GuitarName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courier_Guitar_GuitarName",
                        column: x => x.GuitarName,
                        principalTable: "Guitar",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courier_Shop_ShopName",
                        column: x => x.ShopName,
                        principalTable: "Shop",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuitarName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShopName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_Guitar_GuitarName",
                        column: x => x.GuitarName,
                        principalTable: "Guitar",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_Purchase_Shop_ShopName",
                        column: x => x.ShopName,
                        principalTable: "Shop",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courier_GuitarName",
                table: "Courier",
                column: "GuitarName");

            migrationBuilder.CreateIndex(
                name: "IX_Courier_ShopName",
                table: "Courier",
                column: "ShopName");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_GuitarName",
                table: "Purchase",
                column: "GuitarName");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_ShopName",
                table: "Purchase",
                column: "ShopName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courier");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "Shop");
        }
    }
}
