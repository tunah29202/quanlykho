using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class UpdateOrder_Warehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "warehouse_id",
                schema: "public",
                table: "a_order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_a_order_warehouse_id",
                schema: "public",
                table: "a_order",
                column: "warehouse_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_order_a_warehouse_warehouse_id",
                schema: "public",
                table: "a_order",
                column: "warehouse_id",
                principalSchema: "public",
                principalTable: "a_warehouse",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_order_a_warehouse_warehouse_id",
                schema: "public",
                table: "a_order");

            migrationBuilder.DropIndex(
                name: "IX_a_order_warehouse_id",
                schema: "public",
                table: "a_order");

            migrationBuilder.DropColumn(
                name: "warehouse_id",
                schema: "public",
                table: "a_order");
        }
    }
}
