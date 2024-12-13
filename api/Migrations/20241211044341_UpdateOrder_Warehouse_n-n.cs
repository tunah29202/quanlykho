using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class UpdateOrder_Warehouse_nn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "a_order_warehouse",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_order_warehouse", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_order_warehouse_a_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "public",
                        principalTable: "a_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_order_warehouse_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "public",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_order_warehouse_order_id",
                schema: "public",
                table: "a_order_warehouse",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_warehouse_warehouse_id",
                schema: "public",
                table: "a_order_warehouse",
                column: "warehouse_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "a_order_warehouse",
                schema: "public");

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
    }
}
