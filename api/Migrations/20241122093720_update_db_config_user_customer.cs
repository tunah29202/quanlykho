using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class update_db_config_user_customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                schema: "public",
                table: "a_user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "public",
                table: "a_customer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_a_user_customer_id",
                schema: "public",
                table: "a_user",
                column: "customer_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_a_user_a_customer_customer_id",
                schema: "public",
                table: "a_user",
                column: "customer_id",
                principalSchema: "public",
                principalTable: "a_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_a_user_a_customer_customer_id",
                schema: "public",
                table: "a_user");

            migrationBuilder.DropIndex(
                name: "IX_a_user_customer_id",
                schema: "public",
                table: "a_user");

            migrationBuilder.DropColumn(
                name: "customer_id",
                schema: "public",
                table: "a_user");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "public",
                table: "a_customer");
        }
    }
}
