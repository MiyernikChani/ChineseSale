using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sale.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Donators_DonatorId",
                table: "Gifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gifts",
                table: "Gifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Gifts",
                newName: "Gift");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Gifts_DonatorId",
                table: "Gift",
                newName: "IX_Gift_DonatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Gifts_CategoryId",
                table: "Gift",
                newName: "IX_Gift_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gift",
                table: "Gift",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_Category_CategoryId",
                table: "Gift",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_Donators_DonatorId",
                table: "Gift",
                column: "DonatorId",
                principalTable: "Donators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gift_Category_CategoryId",
                table: "Gift");

            migrationBuilder.DropForeignKey(
                name: "FK_Gift_Donators_DonatorId",
                table: "Gift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gift",
                table: "Gift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Gift",
                newName: "Gifts");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Gift_DonatorId",
                table: "Gifts",
                newName: "IX_Gifts_DonatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Gift_CategoryId",
                table: "Gifts",
                newName: "IX_Gifts_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gifts",
                table: "Gifts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Donators_DonatorId",
                table: "Gifts",
                column: "DonatorId",
                principalTable: "Donators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
