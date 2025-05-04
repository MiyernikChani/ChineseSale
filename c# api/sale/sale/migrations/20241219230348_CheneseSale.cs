using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChneseSaleApi.Migrations
{
    /// <inheritdoc />
    public partial class CheneseSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftOfDonators");

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CountOfSales",
                table: "Gifts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DonatorId",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.DropIndex(
                name: "IX_Gifts_DonatorId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "DonatorId",
                table: "Gifts");

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountOfSales",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GiftOfDonators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonatorId = table.Column<int>(type: "int", nullable: false),
                    GiftId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftOfDonators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftOfDonators_Donators_DonatorId",
                        column: x => x.DonatorId,
                        principalTable: "Donators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftOfDonators_Gifts_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiftOfDonators_DonatorId",
                table: "GiftOfDonators",
                column: "DonatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftOfDonators_GiftId",
                table: "GiftOfDonators",
                column: "GiftId");
        }
    }
}
