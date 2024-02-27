using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationDbContextV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblOrderEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderItemEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderItemEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblOrderItemEntities_tblOrderEntities_OrderId",
                        column: x => x.OrderId,
                        principalTable: "tblOrderEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblOrderItemEntities_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItemEntities_OrderId",
                table: "tblOrderItemEntities",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItemEntities_ProductId",
                table: "tblOrderItemEntities",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOrderItemEntities");

            migrationBuilder.DropTable(
                name: "tblOrderEntities");
        }
    }
}
