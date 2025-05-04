using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChneseSaleApi.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Customers_UserId",
                table: "Winners");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Winners",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Winners_UserId",
                table: "Winners",
                newName: "IX_Winners_CustomerId");

            migrationBuilder.CreateTable(
                name: "totalRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Revenue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_totalRevenues", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Customers_CustomerId",
                table: "Winners",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Customers_CustomerId",
                table: "Winners");

            migrationBuilder.DropTable(
                name: "totalRevenues");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Winners",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Winners_CustomerId",
                table: "Winners",
                newName: "IX_Winners_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Customers_UserId",
                table: "Winners",
                column: "UserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
