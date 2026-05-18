using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MapIdentityUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_asp_net_users_CustomerId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_asp_net_role_claims_asp_net_roles_RoleId",
                table: "asp_net_role_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_asp_net_user_claims_asp_net_users_UserId",
                table: "asp_net_user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_asp_net_user_logins_asp_net_users_UserId",
                table: "asp_net_user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_asp_net_user_roles_asp_net_roles_RoleId",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_asp_net_user_roles_asp_net_users_UserId",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_asp_net_user_tokens_asp_net_users_UserId",
                table: "asp_net_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_asp_net_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_part_requests_asp_net_users_CustomerId",
                table: "part_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_asp_net_users_CustomerId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_asp_net_users_CustomerId",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoices_asp_net_users_CustomerId",
                table: "sales_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_asp_net_users_UserId",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_users",
                table: "asp_net_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_user_tokens",
                table: "asp_net_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_user_roles",
                table: "asp_net_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_user_logins",
                table: "asp_net_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_user_claims",
                table: "asp_net_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_roles",
                table: "asp_net_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asp_net_role_claims",
                table: "asp_net_role_claims");

            migrationBuilder.RenameTable(
                name: "asp_net_users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_user_tokens",
                newName: "user_tokens");

            migrationBuilder.RenameTable(
                name: "asp_net_user_roles",
                newName: "user_roles");

            migrationBuilder.RenameTable(
                name: "asp_net_user_logins",
                newName: "user_logins");

            migrationBuilder.RenameTable(
                name: "asp_net_user_claims",
                newName: "user_claims");

            migrationBuilder.RenameTable(
                name: "asp_net_roles",
                newName: "roles");

            migrationBuilder.RenameTable(
                name: "asp_net_role_claims",
                newName: "role_claims");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_user_roles_RoleId",
                table: "user_roles",
                newName: "IX_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_user_logins_UserId",
                table: "user_logins",
                newName: "IX_user_logins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_user_claims_UserId",
                table: "user_claims",
                newName: "IX_user_claims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_role_claims_RoleId",
                table: "role_claims",
                newName: "IX_role_claims_RoleId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditBalance",
                table: "users",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreditDueDate",
                table: "users",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoyaltyPoints",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManagedBy",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHashText",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSpent",
                table: "users",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_tokens",
                table: "user_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_logins",
                table: "user_logins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_claims",
                table: "user_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                table: "roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_claims",
                table: "role_claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_CustomerId",
                table: "appointments",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_requests_users_CustomerId",
                table: "part_requests",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_users_CustomerId",
                table: "reviews",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_claims_roles_RoleId",
                table: "role_claims",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_users_CustomerId",
                table: "sales",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_invoices_users_CustomerId",
                table: "sales_invoices",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_users_UserId",
                table: "user_claims",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_users_UserId",
                table: "user_logins",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_RoleId",
                table: "user_roles",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_users_UserId",
                table: "user_tokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_users_UserId",
                table: "vehicles",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_CustomerId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_part_requests_users_CustomerId",
                table: "part_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_CustomerId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_role_claims_roles_RoleId",
                table: "role_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_users_CustomerId",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoices_users_CustomerId",
                table: "sales_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_users_UserId",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_users_UserId",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_RoleId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_UserId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_users_UserId",
                table: "user_tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_users_UserId",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_tokens",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_logins",
                table: "user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_claims",
                table: "user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_claims",
                table: "role_claims");

            migrationBuilder.DropColumn(
                name: "CreditBalance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CreditDueDate",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LoyaltyPoints",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ManagedBy",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PasswordHashText",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "users");

            migrationBuilder.DropColumn(
                name: "TotalSpent",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "asp_net_users");

            migrationBuilder.RenameTable(
                name: "user_tokens",
                newName: "asp_net_user_tokens");

            migrationBuilder.RenameTable(
                name: "user_roles",
                newName: "asp_net_user_roles");

            migrationBuilder.RenameTable(
                name: "user_logins",
                newName: "asp_net_user_logins");

            migrationBuilder.RenameTable(
                name: "user_claims",
                newName: "asp_net_user_claims");

            migrationBuilder.RenameTable(
                name: "roles",
                newName: "asp_net_roles");

            migrationBuilder.RenameTable(
                name: "role_claims",
                newName: "asp_net_role_claims");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_RoleId",
                table: "asp_net_user_roles",
                newName: "IX_asp_net_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_user_logins_UserId",
                table: "asp_net_user_logins",
                newName: "IX_asp_net_user_logins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_user_claims_UserId",
                table: "asp_net_user_claims",
                newName: "IX_asp_net_user_claims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_role_claims_RoleId",
                table: "asp_net_role_claims",
                newName: "IX_asp_net_role_claims_RoleId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "asp_net_users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "asp_net_users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "asp_net_users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_users",
                table: "asp_net_users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_user_tokens",
                table: "asp_net_user_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_user_roles",
                table: "asp_net_user_roles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_user_logins",
                table: "asp_net_user_logins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_user_claims",
                table: "asp_net_user_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_roles",
                table: "asp_net_roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_role_claims",
                table: "asp_net_role_claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_asp_net_users_CustomerId",
                table: "appointments",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_asp_net_role_claims_asp_net_roles_RoleId",
                table: "asp_net_role_claims",
                column: "RoleId",
                principalTable: "asp_net_roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asp_net_user_claims_asp_net_users_UserId",
                table: "asp_net_user_claims",
                column: "UserId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asp_net_user_logins_asp_net_users_UserId",
                table: "asp_net_user_logins",
                column: "UserId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asp_net_user_roles_asp_net_roles_RoleId",
                table: "asp_net_user_roles",
                column: "RoleId",
                principalTable: "asp_net_roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asp_net_user_roles_asp_net_users_UserId",
                table: "asp_net_user_roles",
                column: "UserId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asp_net_user_tokens_asp_net_users_UserId",
                table: "asp_net_user_tokens",
                column: "UserId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_asp_net_users_UserId",
                table: "notifications",
                column: "UserId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_requests_asp_net_users_CustomerId",
                table: "part_requests",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_asp_net_users_CustomerId",
                table: "reviews",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_asp_net_users_CustomerId",
                table: "sales",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_invoices_asp_net_users_CustomerId",
                table: "sales_invoices",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_asp_net_users_UserId",
                table: "vehicles",
                column: "UserId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
