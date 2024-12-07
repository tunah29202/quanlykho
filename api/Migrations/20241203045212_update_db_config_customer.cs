using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class update_db_config_customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_invoice_a_bank_account_bank_account_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.DropTable(
                name: "a_bank_account",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_bank_branch",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_bank_name",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_card_type",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_a_invoice_bank_account_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.DropColumn(
                name: "bank_account_id",
                schema: "public",
                table: "a_invoice");

            migrationBuilder.DropColumn(
                name: "code",
                schema: "public",
                table: "a_customer");

            migrationBuilder.DropColumn(
                name: "company_type",
                schema: "public",
                table: "a_customer");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                schema: "public",
                table: "a_order",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "payment_method_id",
                schema: "public",
                table: "a_order",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_a_order_payment_method_id",
                schema: "public",
                table: "a_order",
                column: "payment_method_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_order_a_payment_method_payment_method_id",
                schema: "public",
                table: "a_order",
                column: "payment_method_id",
                principalSchema: "public",
                principalTable: "a_payment_method",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_order_a_payment_method_payment_method_id",
                schema: "public",
                table: "a_order");

            migrationBuilder.DropIndex(
                name: "IX_a_order_payment_method_id",
                schema: "public",
                table: "a_order");

            migrationBuilder.DropColumn(
                name: "payment_method_id",
                schema: "public",
                table: "a_order");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "public",
                table: "a_order",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "bank_account_id",
                schema: "public",
                table: "a_invoice",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                schema: "public",
                table: "a_customer",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "company_type",
                schema: "public",
                table: "a_customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "a_bank_branch",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_bank_branch", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_bank_name",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_bank_name", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_card_type",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_card_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_bank_account",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_name_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    card_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_bank_account", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_bank_account_a_bank_branch_bank_branch_id",
                        column: x => x.bank_branch_id,
                        principalSchema: "public",
                        principalTable: "a_bank_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_bank_account_a_bank_name_bank_name_id",
                        column: x => x.bank_name_id,
                        principalSchema: "public",
                        principalTable: "a_bank_name",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_bank_account_a_card_type_card_type_id",
                        column: x => x.card_type_id,
                        principalSchema: "public",
                        principalTable: "a_card_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_bank_account_a_payment_method_payment_method_id",
                        column: x => x.payment_method_id,
                        principalSchema: "public",
                        principalTable: "a_payment_method",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_bank_account_id",
                schema: "public",
                table: "a_invoice",
                column: "bank_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_bank_account_bank_branch_id",
                schema: "public",
                table: "a_bank_account",
                column: "bank_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_bank_account_bank_name_id",
                schema: "public",
                table: "a_bank_account",
                column: "bank_name_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_bank_account_card_type_id",
                schema: "public",
                table: "a_bank_account",
                column: "card_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_bank_account_payment_method_id",
                schema: "public",
                table: "a_bank_account",
                column: "payment_method_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_invoice_a_bank_account_bank_account_id",
                schema: "public",
                table: "a_invoice",
                column: "bank_account_id",
                principalSchema: "public",
                principalTable: "a_bank_account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
