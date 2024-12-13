using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class UpdateOrder_Invoice_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_a_invoice_order_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_order_id",
                schema: "public",
                table: "a_invoice",
                column: "order_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_a_invoice_order_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_order_id",
                schema: "public",
                table: "a_invoice",
                column: "order_id",
                unique: true);
        }
    }
}
