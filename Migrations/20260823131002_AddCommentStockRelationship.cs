using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentStockRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StockId",
                table: "Comments",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stock_StockId",
                table: "Comments",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stock_StockId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_StockId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "Comments");
        }
    }
}
