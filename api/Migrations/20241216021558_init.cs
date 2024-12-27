using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "a_category",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_customer",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    tax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_function",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    parent_cd = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_function", x => x.id);
                    table.UniqueConstraint("AK_a_function_code", x => x.code);
                    table.ForeignKey(
                        name: "FK_a_function_a_function_parent_cd",
                        column: x => x.parent_cd,
                        principalSchema: "dbo",
                        principalTable: "a_function",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_payment_method",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_payment_method", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_role",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    description = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_role", x => x.id);
                    table.UniqueConstraint("AK_a_role_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "a_shipper",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    tel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    fax = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_shipper", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_warehouse",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    tel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_warehouse", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_log_exception",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    function = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stack_trace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_log_exception", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_resource",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lang = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    screen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_resource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_ingredient",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_ingredient", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_ingredient_a_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "dbo",
                        principalTable: "a_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    hash_password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    salt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    verification_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_a_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "dbo",
                        principalTable: "a_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_order",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_no = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    payment_method_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_order_a_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "dbo",
                        principalTable: "a_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_order_a_payment_method_payment_method_id",
                        column: x => x.payment_method_id,
                        principalSchema: "dbo",
                        principalTable: "a_payment_method",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_permission",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_cd = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    function_cd = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_permission_a_function_function_cd",
                        column: x => x.function_cd,
                        principalSchema: "dbo",
                        principalTable: "a_function",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_permission_a_role_role_cd",
                        column: x => x.role_cd,
                        principalSchema: "dbo",
                        principalTable: "a_role",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_carton",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    carton_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    net_weight = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    gross_weight = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    length = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    height = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    width = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    volume = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_carton", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_carton_a_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "dbo",
                        principalTable: "a_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_carton_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "dbo",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_product",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    image = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    origin = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    price_unit = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, nullable: false),
                    ingredient = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_product_a_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "dbo",
                        principalTable: "a_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_product_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "dbo",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user_role",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_cd = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user_role", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_role_a_role_role_cd",
                        column: x => x.role_cd,
                        principalSchema: "dbo",
                        principalTable: "a_role",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_user_role_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user_warehouse",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user_warehouse", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_warehouse_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_user_warehouse_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "dbo",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "m_log_action",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_log_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_log_action_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_order_warehouse",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_order_warehouse", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_order_warehouse_a_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "dbo",
                        principalTable: "a_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_order_warehouse_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "dbo",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_invoice",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_no = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "datetime2", maxLength: 20, nullable: true),
                    shipped_per = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    shipped_date = table.Column<DateTime>(type: "datetime2", maxLength: 20, nullable: true),
                    total_weight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    total_volumn = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipper_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    carton_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    payment_method_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_invoice", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_carton_carton_id",
                        column: x => x.carton_id,
                        principalSchema: "dbo",
                        principalTable: "a_carton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "dbo",
                        principalTable: "a_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_payment_method_payment_method_id",
                        column: x => x.payment_method_id,
                        principalSchema: "dbo",
                        principalTable: "a_payment_method",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_shipper_shipper_id",
                        column: x => x.shipper_id,
                        principalSchema: "dbo",
                        principalTable: "a_shipper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "dbo",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_carton_detail",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    carton_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_carton_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_carton_detail_a_carton_carton_id",
                        column: x => x.carton_id,
                        principalSchema: "dbo",
                        principalTable: "a_carton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_carton_detail_a_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "dbo",
                        principalTable: "a_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_order_detail",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    del_flg = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_order_detail_a_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "dbo",
                        principalTable: "a_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_order_detail_a_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "dbo",
                        principalTable: "a_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_customer_id",
                schema: "dbo",
                table: "a_carton",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_warehouse_id",
                schema: "dbo",
                table: "a_carton",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_detail_carton_id",
                schema: "dbo",
                table: "a_carton_detail",
                column: "carton_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_detail_product_id",
                schema: "dbo",
                table: "a_carton_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_function_parent_cd",
                schema: "dbo",
                table: "a_function",
                column: "parent_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_ingredient_category_id",
                schema: "dbo",
                table: "a_ingredient",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_carton_id",
                schema: "dbo",
                table: "a_invoice",
                column: "carton_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_order_id",
                schema: "dbo",
                table: "a_invoice",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_payment_method_id",
                schema: "dbo",
                table: "a_invoice",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_shipper_id",
                schema: "dbo",
                table: "a_invoice",
                column: "shipper_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_warehouse_id",
                schema: "dbo",
                table: "a_invoice",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_customer_id",
                schema: "dbo",
                table: "a_order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_payment_method_id",
                schema: "dbo",
                table: "a_order",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_detail_order_id",
                schema: "dbo",
                table: "a_order_detail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_detail_product_id",
                schema: "dbo",
                table: "a_order_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_warehouse_order_id",
                schema: "dbo",
                table: "a_order_warehouse",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_order_warehouse_warehouse_id",
                schema: "dbo",
                table: "a_order_warehouse",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_permission_function_cd",
                schema: "dbo",
                table: "a_permission",
                column: "function_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_permission_role_cd",
                schema: "dbo",
                table: "a_permission",
                column: "role_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_product_category_id",
                schema: "dbo",
                table: "a_product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_product_warehouse_id",
                schema: "dbo",
                table: "a_product",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_customer_id",
                schema: "dbo",
                table: "a_user",
                column: "customer_id",
                unique: true,
                filter: "[customer_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_role_role_cd",
                schema: "dbo",
                table: "a_user_role",
                column: "role_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_role_user_id",
                schema: "dbo",
                table: "a_user_role",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_a_user_warehouse_user_id",
                schema: "dbo",
                table: "a_user_warehouse",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_warehouse_warehouse_id",
                schema: "dbo",
                table: "a_user_warehouse",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_log_action_user_id",
                schema: "dbo",
                table: "m_log_action",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "a_carton_detail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_ingredient",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_invoice",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_order_detail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_order_warehouse",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_permission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_user_role",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_user_warehouse",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "m_log_action",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "m_log_exception",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "m_resource",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_carton",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_shipper",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_product",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_order",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_function",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_role",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_user",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_category",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_warehouse",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_payment_method",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "a_customer",
                schema: "dbo");
        }
    }
}
