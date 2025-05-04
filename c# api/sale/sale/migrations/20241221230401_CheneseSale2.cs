using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChneseSaleApi.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Donators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Donators");
        }
    }
}
