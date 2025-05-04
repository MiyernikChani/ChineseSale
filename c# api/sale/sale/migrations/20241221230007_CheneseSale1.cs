using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChneseSaleApi.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Donators");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Donators");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Donators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Donators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
