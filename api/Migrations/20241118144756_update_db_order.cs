using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class update_db_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_invoice_a_consignee_consignee_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.DropTable(
                name: "a_consignee",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_a_invoice_consignee_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.RenameColumn(
                name: "consignee_id",
                schema: "public",
                table: "a_invoice",
                newName: "order_id");

            migrationBuilder.AddColumn<string>(
                name: "company_name",
                schema: "public",
                table: "a_customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "company_type",
                schema: "public",
                table: "a_customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                schema: "public",
                table: "a_customer",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "public",
                table: "a_customer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tax",
                schema: "public",
                table: "a_customer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "userid",
                schema: "public",
                table: "a_customer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "a_order",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    order_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_order_a_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "public",
                        principalTable: "a_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_order_detail",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_order_detail_a_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "public",
                        principalTable: "a_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_order_detail_a_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "public",
                        principalTable: "a_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_order_id",
                schema: "public",
                table: "a_invoice",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_a_customer_userid",
                schema: "public",
                table: "a_customer",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_customer_id",
                schema: "public",
                table: "a_order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_detail_order_id",
                schema: "public",
                table: "a_order_detail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_detail_product_id",
                schema: "public",
                table: "a_order_detail",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_customer_a_user_userid",
                schema: "public",
                table: "a_customer",
                column: "userid",
                principalSchema: "public",
                principalTable: "a_user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_invoice_a_order_order_id",
                schema: "public",
                table: "a_invoice",
                column: "order_id",
                principalSchema: "public",
                principalTable: "a_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_customer_a_user_userid",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropForeignKey(
                name: "FK_a_invoice_a_order_order_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.DropTable(
                name: "a_order_detail",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_order",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_a_invoice_order_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.DropIndex(
                name: "IX_a_customer_userid",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "company_name",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "company_type",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "customer_id",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "tax",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "userid",
                schema: "public",
                table: "a_customer");

            migrationBuilder.RenameColumn(
                name: "order_id",
                schema: "public",
                table: "a_invoice",
                newName: "consignee_id");

            migrationBuilder.CreateTable(
                name: "a_consignee",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    fax = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tax = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    tel = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_consignee", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_consignee_id",
                schema: "public",
                table: "a_invoice",
                column: "consignee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_invoice_a_consignee_consignee_id",
                schema: "public",
                table: "a_invoice",
                column: "consignee_id",
                principalSchema: "public",
                principalTable: "a_consignee",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
