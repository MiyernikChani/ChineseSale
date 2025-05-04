using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChneseSaleApi.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donators", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_DonatorId",
                table: "Gifts",
                column: "DonatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Donators_DonatorId",
                table: "Gifts",
                column: "DonatorId",
                principalTable: "Donators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Donators_DonatorId",
                table: "Gifts");

            migrationBuilder.DropTable(
                name: "Donators");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_DonatorId",
                table: "Gifts");
        }
    }
}
