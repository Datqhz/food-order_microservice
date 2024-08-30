using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class AjustRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                schema: "order",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_EaterId",
                schema: "order",
                table: "Order",
                column: "EaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MerchantId",
                schema: "order",
                table: "Order",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_EaterId",
                schema: "order",
                table: "Order",
                column: "EaterId",
                principalSchema: "order",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_MerchantId",
                schema: "order",
                table: "Order",
                column: "MerchantId",
                principalSchema: "order",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_EaterId",
                schema: "order",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_MerchantId",
                schema: "order",
                table: "Order");

            migrationBuilder.DropTable(
                name: "User",
                schema: "order");

            migrationBuilder.DropIndex(
                name: "IX_Order_EaterId",
                schema: "order",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_MerchantId",
                schema: "order",
                table: "Order");
        }
    }
}
