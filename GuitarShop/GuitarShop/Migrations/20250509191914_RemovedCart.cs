using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuitarShop.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "Guitar",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guitar_UsersId",
                table: "Guitar",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guitar_Users_UsersId",
                table: "Guitar",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guitar_Users_UsersId",
                table: "Guitar");

            migrationBuilder.DropIndex(
                name: "IX_Guitar_UsersId",
                table: "Guitar");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Guitar");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuitarName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Guitar_GuitarName",
                        column: x => x.GuitarName,
                        principalTable: "Guitar",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_GuitarName",
                table: "Carts",
                column: "GuitarName");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");
        }
    }
}
