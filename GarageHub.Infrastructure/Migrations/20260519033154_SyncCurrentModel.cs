using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GarageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncCurrentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_CustomerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Vehicles_VehicleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_UserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Sales_SaleId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PartRequests_AspNetUsers_CustomerId",
                table: "PartRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Customers_CustomerId",
                table: "Purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Appointments_AppointmentId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Parts_PartId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_AspNetUsers_CustomerId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItems_Parts_PartId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItems_SalesInvoices_SaleId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_AspNetUsers_CustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Customers_CustomerId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchase",
                table: "Purchase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parts",
                table: "Parts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesInvoices",
                table: "SalesInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesInvoiceItems",
                table: "SalesInvoiceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartRequests",
                table: "PartRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "vehicles");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "sales");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "reviews");

            migrationBuilder.RenameTable(
                name: "Purchase",
                newName: "purchase");

            migrationBuilder.RenameTable(
                name: "Parts",
                newName: "parts");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notifications");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customers");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "appointments");

            migrationBuilder.RenameTable(
                name: "SalesInvoices",
                newName: "sales_invoices");

            migrationBuilder.RenameTable(
                name: "SalesInvoiceItems",
                newName: "sales_invoice_items");

            migrationBuilder.RenameTable(
                name: "SaleItems",
                newName: "sale_items");

            migrationBuilder.RenameTable(
                name: "PartRequests",
                newName: "part_requests");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_UserId",
                table: "vehicles",
                newName: "IX_vehicles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_CustomerId",
                table: "vehicles",
                newName: "IX_vehicles_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_CustomerId",
                table: "sales",
                newName: "IX_sales_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CustomerId",
                table: "reviews",
                newName: "IX_reviews_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_AppointmentId",
                table: "reviews",
                newName: "IX_reviews_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_CustomerId",
                table: "purchase",
                newName: "IX_purchase_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "notifications",
                newName: "IX_notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_UserId",
                table: "customers",
                newName: "IX_customers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_VehicleId",
                table: "appointments",
                newName: "IX_appointments_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CustomerId",
                table: "appointments",
                newName: "IX_appointments_CustomerId");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "sales_invoices",
                newName: "subtotal");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "sales_invoices",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "sales_invoices",
                newName: "staff_id");

            migrationBuilder.RenameColumn(
                name: "SaleDate",
                table: "sales_invoices",
                newName: "sale_date");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "sales_invoices",
                newName: "payment_status");

            migrationBuilder.RenameColumn(
                name: "DiscountApplied",
                table: "sales_invoices",
                newName: "discount_applied");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "sales_invoices",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreditUsed",
                table: "sales_invoices",
                newName: "credit_used");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                table: "sales_invoices",
                newName: "sale_id");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoices_CustomerId",
                table: "sales_invoices",
                newName: "IX_sales_invoices_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoiceItems_SaleId",
                table: "sales_invoice_items",
                newName: "IX_sales_invoice_items_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoiceItems_PartId",
                table: "sales_invoice_items",
                newName: "IX_sales_invoice_items_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItems_SaleId",
                table: "sale_items",
                newName: "IX_sale_items_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItems_PartId",
                table: "sale_items",
                newName: "IX_sale_items_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_PartRequests_CustomerId",
                table: "part_requests",
                newName: "IX_part_requests_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_SaleId",
                table: "Invoice",
                newName: "IX_Invoice_SaleId");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "users",
                newName: "LoyaltyPoints");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "notifications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "notifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "notifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "sales_invoices",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "sales_invoices",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_applied",
                table: "sales_invoices",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

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
                nullable: false,
                defaultValue: "",
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
                name: "PK_vehicles",
                table: "vehicles",
                column: "VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales",
                table: "sales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reviews",
                table: "reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase",
                table: "purchase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_parts",
                table: "parts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notifications",
                table: "notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appointments",
                table: "appointments",
                column: "AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_invoices",
                table: "sales_invoices",
                column: "sale_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_invoice_items",
                table: "sales_invoice_items",
                column: "ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sale_items",
                table: "sale_items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_part_requests",
                table: "part_requests",
                column: "RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_CustomerId",
                table: "appointments",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_vehicles_VehicleId",
                table: "appointments",
                column: "VehicleId",
                principalTable: "vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_customers_users_UserId",
                table: "customers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_sales_SaleId",
                table: "Invoice",
                column: "SaleId",
                principalTable: "sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_requests_users_CustomerId",
                table: "part_requests",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_customers_CustomerId",
                table: "purchase",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_appointments_AppointmentId",
                table: "reviews",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_users_CustomerId",
                table: "reviews",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sale_items_parts_PartId",
                table: "sale_items",
                column: "PartId",
                principalTable: "parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sale_items_sales_SaleId",
                table: "sale_items",
                column: "SaleId",
                principalTable: "sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_users_CustomerId",
                table: "sales",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_invoice_items_parts_PartId",
                table: "sales_invoice_items",
                column: "PartId",
                principalTable: "parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_invoice_items_sales_invoices_SaleId",
                table: "sales_invoice_items",
                column: "SaleId",
                principalTable: "sales_invoices",
                principalColumn: "sale_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_invoices_users_customer_id",
                table: "sales_invoices",
                column: "customer_id",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_customers_CustomerId",
                table: "vehicles",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_users_UserId",
                table: "vehicles",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_CustomerId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_vehicles_VehicleId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_customers_users_UserId",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_sales_SaleId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_part_requests_users_CustomerId",
                table: "part_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_customers_CustomerId",
                table: "purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_appointments_AppointmentId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_CustomerId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_parts_PartId",
                table: "sale_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_sales_SaleId",
                table: "sale_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_users_CustomerId",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoice_items_parts_PartId",
                table: "sales_invoice_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoice_items_sales_invoices_SaleId",
                table: "sales_invoice_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoices_users_customer_id",
                table: "sales_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_customers_CustomerId",
                table: "vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_users_UserId",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales",
                table: "sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reviews",
                table: "reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase",
                table: "purchase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_parts",
                table: "parts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notifications",
                table: "notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appointments",
                table: "appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_invoices",
                table: "sales_invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_invoice_items",
                table: "sales_invoice_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sale_items",
                table: "sale_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_part_requests",
                table: "part_requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "CreditBalance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CreditDueDate",
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
                name: "vehicles",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "sales",
                newName: "Sales");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "purchase",
                newName: "Purchase");

            migrationBuilder.RenameTable(
                name: "parts",
                newName: "Parts");

            migrationBuilder.RenameTable(
                name: "notifications",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "appointments",
                newName: "Appointments");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "sales_invoices",
                newName: "SalesInvoices");

            migrationBuilder.RenameTable(
                name: "sales_invoice_items",
                newName: "SalesInvoiceItems");

            migrationBuilder.RenameTable(
                name: "sale_items",
                newName: "SaleItems");

            migrationBuilder.RenameTable(
                name: "part_requests",
                newName: "PartRequests");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_UserId",
                table: "Vehicles",
                newName: "IX_Vehicles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_CustomerId",
                table: "Vehicles",
                newName: "IX_Vehicles_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_sales_CustomerId",
                table: "Sales",
                newName: "IX_Sales_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_CustomerId",
                table: "Reviews",
                newName: "IX_Reviews_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_AppointmentId",
                table: "Reviews",
                newName: "IX_Reviews_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_CustomerId",
                table: "Purchase",
                newName: "IX_Purchase_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_notifications_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_customers_UserId",
                table: "Customers",
                newName: "IX_Customers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_VehicleId",
                table: "Appointments",
                newName: "IX_Appointments_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_CustomerId",
                table: "Appointments",
                newName: "IX_Appointments_CustomerId");

            migrationBuilder.RenameColumn(
                name: "LoyaltyPoints",
                table: "AspNetUsers",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "subtotal",
                table: "SalesInvoices",
                newName: "Subtotal");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "SalesInvoices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "staff_id",
                table: "SalesInvoices",
                newName: "StaffId");

            migrationBuilder.RenameColumn(
                name: "sale_date",
                table: "SalesInvoices",
                newName: "SaleDate");

            migrationBuilder.RenameColumn(
                name: "payment_status",
                table: "SalesInvoices",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "discount_applied",
                table: "SalesInvoices",
                newName: "DiscountApplied");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "SalesInvoices",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "credit_used",
                table: "SalesInvoices",
                newName: "CreditUsed");

            migrationBuilder.RenameColumn(
                name: "sale_id",
                table: "SalesInvoices",
                newName: "SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_sales_invoices_customer_id",
                table: "SalesInvoices",
                newName: "IX_SalesInvoices_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_sales_invoice_items_SaleId",
                table: "SalesInvoiceItems",
                newName: "IX_SalesInvoiceItems_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_sales_invoice_items_PartId",
                table: "SalesInvoiceItems",
                newName: "IX_SalesInvoiceItems_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_sale_items_SaleId",
                table: "SaleItems",
                newName: "IX_SaleItems_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_sale_items_PartId",
                table: "SaleItems",
                newName: "IX_SaleItems_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_part_requests_CustomerId",
                table: "PartRequests",
                newName: "IX_PartRequests_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_SaleId",
                table: "Invoices",
                newName: "IX_Invoices_SaleId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Subtotal",
                table: "SalesInvoices",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<float>(
                name: "TotalAmount",
                table: "SalesInvoices",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<float>(
                name: "DiscountApplied",
                table: "SalesInvoices",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchase",
                table: "Purchase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parts",
                table: "Parts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesInvoices",
                table: "SalesInvoices",
                column: "SaleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesInvoiceItems",
                table: "SalesInvoiceItems",
                column: "ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartRequests",
                table: "PartRequests",
                column: "RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_CustomerId",
                table: "Appointments",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Vehicles_VehicleId",
                table: "Appointments",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Sales_SaleId",
                table: "Invoices",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartRequests_AspNetUsers_CustomerId",
                table: "PartRequests",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Customers_CustomerId",
                table: "Purchase",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Appointments_AppointmentId",
                table: "Reviews",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Parts_PartId",
                table: "SaleItems",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_AspNetUsers_CustomerId",
                table: "Sales",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_Parts_PartId",
                table: "SalesInvoiceItems",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_SalesInvoices_SaleId",
                table: "SalesInvoiceItems",
                column: "SaleId",
                principalTable: "SalesInvoices",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_AspNetUsers_CustomerId",
                table: "SalesInvoices",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Customers_CustomerId",
                table: "Vehicles",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
