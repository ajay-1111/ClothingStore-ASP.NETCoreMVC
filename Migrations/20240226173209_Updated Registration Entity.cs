using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRegistrationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "tblUserRegistration");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "tblUserRegistration",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "tblUserRegistration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "tblUserRegistration",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
