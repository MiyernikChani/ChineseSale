using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChneseSaleApi.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Customers_UserId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Purchases",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                newName: "IX_Purchases_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Customers_CustomerId",
                table: "Purchases",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Customers_CustomerId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Purchases",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_CustomerId",
                table: "Purchases",
                newName: "IX_Purchases_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Customers_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
