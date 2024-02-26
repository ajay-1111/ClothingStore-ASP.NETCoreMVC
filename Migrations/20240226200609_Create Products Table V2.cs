using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductsTableV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "tblUserRegistration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "tblUserRegistration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "tblUserRegistration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "tblUserRegistration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "tblUserRegistration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "tblUserRegistration");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "tblUserRegistration");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "tblUserRegistration");

            migrationBuilder.DropColumn(
                name: "County",
                table: "tblUserRegistration");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "tblUserRegistration");
        }
    }
}
