using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationDbContextV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "tblOrderEntities");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "tblOrderEntities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "tblOrderEntities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "tblOrderEntities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
