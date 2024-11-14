using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "a_bank_branch",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
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
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
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
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_card_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_category",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_consignee",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tax = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    tel = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    fax = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_consignee", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_customer",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tel = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_function",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    method = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    parent_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_function", x => x.id);
                    table.UniqueConstraint("AK_a_function_code", x => x.code);
                    table.ForeignKey(
                        name: "FK_a_function_a_function_parent_cd",
                        column: x => x.parent_cd,
                        principalSchema: "public",
                        principalTable: "a_function",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_payment_method",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_payment_method", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_role",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    description = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_role", x => x.id);
                    table.UniqueConstraint("AK_a_role_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "a_shipper",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tel = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    fax = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_shipper", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_user",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    hash_password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    salt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    verification_code = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_warehouse",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tel = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_warehouse", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_log_exception",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    function = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    stack_trace = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_log_exception", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_resource",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lang = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    module = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    screen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_resource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_ingredient",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_ingredient", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_ingredient_a_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "public",
                        principalTable: "a_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_bank_account",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_name_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    card_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "a_permission",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    function_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_permission_a_function_function_cd",
                        column: x => x.function_cd,
                        principalSchema: "public",
                        principalTable: "a_function",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_permission_a_role_role_cd",
                        column: x => x.role_cd,
                        principalSchema: "public",
                        principalTable: "a_role",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user_role",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user_role", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_role_a_role_role_cd",
                        column: x => x.role_cd,
                        principalSchema: "public",
                        principalTable: "a_role",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_user_role_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "m_log_action",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_log_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_log_action_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_carton",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    carton_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    net_weight = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    gross_weight = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    length = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    height = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    width = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    volume = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_carton", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_carton_a_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "public",
                        principalTable: "a_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_carton_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "public",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_package",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    origin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "a_product",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    image = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    origin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    price_unit = table.Column<decimal>(type: "numeric", maxLength: 20, nullable: false),
                    ingredient = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_product_a_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "public",
                        principalTable: "a_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_product_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "public",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user_warehouse",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user_warehouse", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_warehouse_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_user_warehouse_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "public",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_invoice",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "timestamp without time zone", maxLength: 20, nullable: true),
                    shipped_per = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    shipped_date = table.Column<DateTime>(type: "timestamp without time zone", maxLength: 20, nullable: true),
                    total_weight = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    total_volumn = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    shipper_id = table.Column<Guid>(type: "uuid", nullable: false),
                    consignee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    carton_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    payment_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_invoice", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_bank_account_bank_account_id",
                        column: x => x.bank_account_id,
                        principalSchema: "public",
                        principalTable: "a_bank_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_carton_carton_id",
                        column: x => x.carton_id,
                        principalSchema: "public",
                        principalTable: "a_carton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_consignee_consignee_id",
                        column: x => x.consignee_id,
                        principalSchema: "public",
                        principalTable: "a_consignee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_payment_method_payment_method_id",
                        column: x => x.payment_method_id,
                        principalSchema: "public",
                        principalTable: "a_payment_method",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_shipper_shipper_id",
                        column: x => x.shipper_id,
                        principalSchema: "public",
                        principalTable: "a_shipper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_invoice_a_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "public",
                        principalTable: "a_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_carton_detail",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    carton_id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_a_carton_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_carton_detail_a_carton_carton_id",
                        column: x => x.carton_id,
                        principalSchema: "public",
                        principalTable: "a_carton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_carton_detail_a_package_package_id",
                        column: x => x.package_id,
                        principalSchema: "public",
                        principalTable: "a_package",
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

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_customer_id",
                schema: "public",
                table: "a_carton",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_warehouse_id",
                schema: "public",
                table: "a_carton",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_detail_carton_id",
                schema: "public",
                table: "a_carton_detail",
                column: "carton_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_carton_detail_package_id",
                schema: "public",
                table: "a_carton_detail",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_function_parent_cd",
                schema: "public",
                table: "a_function",
                column: "parent_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_ingredient_category_id",
                schema: "public",
                table: "a_ingredient",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_bank_account_id",
                schema: "public",
                table: "a_invoice",
                column: "bank_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_carton_id",
                schema: "public",
                table: "a_invoice",
                column: "carton_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_consignee_id",
                schema: "public",
                table: "a_invoice",
                column: "consignee_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_payment_method_id",
                schema: "public",
                table: "a_invoice",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_shipper_id",
                schema: "public",
                table: "a_invoice",
                column: "shipper_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_invoice_warehouse_id",
                schema: "public",
                table: "a_invoice",
                column: "warehouse_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_a_permission_function_cd",
                schema: "public",
                table: "a_permission",
                column: "function_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_permission_role_cd",
                schema: "public",
                table: "a_permission",
                column: "role_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_product_category_id",
                schema: "public",
                table: "a_product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_product_warehouse_id",
                schema: "public",
                table: "a_product",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_role_role_cd",
                schema: "public",
                table: "a_user_role",
                column: "role_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_role_user_id",
                schema: "public",
                table: "a_user_role",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_a_user_warehouse_user_id",
                schema: "public",
                table: "a_user_warehouse",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_warehouse_warehouse_id",
                schema: "public",
                table: "a_user_warehouse",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_log_action_user_id",
                schema: "public",
                table: "m_log_action",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "a_carton_detail",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_ingredient",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_invoice",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_package_detail",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_permission",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_user_role",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_user_warehouse",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_log_action",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_log_exception",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_resource",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_bank_account",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_carton",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_consignee",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_shipper",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_package",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_product",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_function",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_role",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_user",
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

            migrationBuilder.DropTable(
                name: "a_payment_method",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_customer",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_category",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_warehouse",
                schema: "public");
        }
    }
}
