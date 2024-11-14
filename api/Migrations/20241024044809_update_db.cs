using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class update_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_carton_detail_a_package_package_id",
                schema: "public",
                table: "a_carton_detail");

            migrationBuilder.DropTable(
                name: "a_package_detail",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_package",
                schema: "public");

            migrationBuilder.RenameColumn(
                name: "package_id",
                schema: "public",
                table: "a_carton_detail",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_a_carton_detail_package_id",
                schema: "public",
                table: "a_carton_detail",
                newName: "IX_a_carton_detail_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_carton_detail_a_product_product_id",
                schema: "public",
                table: "a_carton_detail",
                column: "product_id",
                principalSchema: "public",
                principalTable: "a_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_carton_detail_a_product_product_id",
                schema: "public",
                table: "a_carton_detail");

            migrationBuilder.RenameColumn(
                name: "product_id",
                schema: "public",
                table: "a_carton_detail",
                newName: "package_id");

            migrationBuilder.RenameIndex(
                name: "IX_a_carton_detail_product_id",
                schema: "public",
                table: "a_carton_detail",
                newName: "IX_a_carton_detail_package_id");

            migrationBuilder.CreateTable(
                name: "a_package",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    origin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    weight = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_package", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_package_a_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "public",
                        principalTable: "a_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_package_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "public",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_package_detail",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_package_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_package_detail_a_package_package_id",
                        column: x => x.package_id,
                        principalSchema: "public",
                        principalTable: "a_package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_package_detail_a_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "public",
                        principalTable: "a_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_package_customer_id",
                schema: "public",
                table: "a_package",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_package_warehouse_id",
                schema: "public",
                table: "a_package",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_package_detail_package_id",
                schema: "public",
                table: "a_package_detail",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_package_detail_product_id",
                schema: "public",
                table: "a_package_detail",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_a_carton_detail_a_package_package_id",
                schema: "public",
                table: "a_carton_detail",
                column: "package_id",
                principalSchema: "public",
                principalTable: "a_package",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
