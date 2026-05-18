using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseSnakeCaseTableNames : Migration
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
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

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

            migrationBuilder.Sql("ALTER TABLE \"SalesInvoiceItems\" DROP CONSTRAINT IF EXISTS \"FK_SalesInvoiceItems_Parts_PartId\"");
            migrationBuilder.Sql("ALTER TABLE \"SalesInvoiceItems\" DROP CONSTRAINT IF EXISTS \"FK_SalesInvoiceItems_SalesInvoices_SaleId\"");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_AspNetUsers_CustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles");

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
                name: "PK_Parts",
                table: "Parts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

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
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

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
                name: "Parts",
                newName: "parts");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notifications");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "invoices");

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
                name: "AspNetUserTokens",
                newName: "asp_net_user_tokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "asp_net_users");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "asp_net_user_roles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "asp_net_user_logins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "asp_net_user_claims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "asp_net_roles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "asp_net_role_claims");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_UserId",
                table: "vehicles",
                newName: "IX_vehicles_UserId");

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
                name: "IX_Notifications_UserId",
                table: "notifications",
                newName: "IX_notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_SaleId",
                table: "invoices",
                newName: "IX_invoices_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_VehicleId",
                table: "appointments",
                newName: "IX_appointments_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CustomerId",
                table: "appointments",
                newName: "IX_appointments_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoices_CustomerId",
                table: "sales_invoices",
                newName: "IX_sales_invoices_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoiceItems_SaleId",
                table: "sales_invoice_items",
                newName: "IX_sales_invoice_items_SaleId");

            migrationBuilder.Sql("ALTER INDEX IF EXISTS \"IX_SalesInvoiceItems_PartId\" RENAME TO \"IX_sales_invoice_items_PartId\"");

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
                name: "IX_AspNetUserRoles_RoleId",
                table: "asp_net_user_roles",
                newName: "IX_asp_net_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "asp_net_user_logins",
                newName: "IX_asp_net_user_logins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "asp_net_user_claims",
                newName: "IX_asp_net_user_claims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "asp_net_role_claims",
                newName: "IX_asp_net_role_claims_RoleId");

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
                name: "PK_parts",
                table: "parts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notifications",
                table: "notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_invoices",
                table: "invoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appointments",
                table: "appointments",
                column: "AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_invoices",
                table: "sales_invoices",
                column: "SaleId");

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
                name: "PK_asp_net_user_tokens",
                table: "asp_net_user_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_asp_net_users",
                table: "asp_net_users",
                column: "Id");

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
                name: "FK_appointments_vehicles_VehicleId",
                table: "appointments",
                column: "VehicleId",
                principalTable: "vehicles",
                principalColumn: "VehicleId",
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
                name: "FK_invoices_sales_SaleId",
                table: "invoices",
                column: "SaleId",
                principalTable: "sales",
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
                name: "FK_reviews_appointments_AppointmentId",
                table: "reviews",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_asp_net_users_CustomerId",
                table: "reviews",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
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
                name: "FK_sales_asp_net_users_CustomerId",
                table: "sales",
                column: "CustomerId",
                principalTable: "asp_net_users",
                principalColumn: "Id",
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
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_asp_net_users_CustomerId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_vehicles_VehicleId",
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
                name: "FK_invoices_sales_SaleId",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_asp_net_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_part_requests_asp_net_users_CustomerId",
                table: "part_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_appointments_AppointmentId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_asp_net_users_CustomerId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_parts_PartId",
                table: "sale_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_sales_SaleId",
                table: "sale_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_asp_net_users_CustomerId",
                table: "sales");

            migrationBuilder.Sql("ALTER TABLE \"sales_invoice_items\" DROP CONSTRAINT IF EXISTS \"FK_sales_invoice_items_parts_PartId\"");
            migrationBuilder.Sql("ALTER TABLE \"sales_invoice_items\" DROP CONSTRAINT IF EXISTS \"FK_sales_invoice_items_sales_invoices_SaleId\"");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoices_asp_net_users_CustomerId",
                table: "sales_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_asp_net_users_UserId",
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
                name: "PK_parts",
                table: "parts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notifications",
                table: "notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_invoices",
                table: "invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appointments",
                table: "appointments");

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
                name: "vehicles",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "sales",
                newName: "Sales");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "parts",
                newName: "Parts");

            migrationBuilder.RenameTable(
                name: "notifications",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "invoices",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "appointments",
                newName: "Appointments");

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
                name: "asp_net_users",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "asp_net_user_tokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "asp_net_user_roles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "asp_net_user_logins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "asp_net_user_claims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "asp_net_roles",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "asp_net_role_claims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_UserId",
                table: "Vehicles",
                newName: "IX_Vehicles_UserId");

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
                name: "IX_notifications_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_invoices_SaleId",
                table: "Invoices",
                newName: "IX_Invoices_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_VehicleId",
                table: "Appointments",
                newName: "IX_Appointments_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_CustomerId",
                table: "Appointments",
                newName: "IX_Appointments_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_sales_invoices_CustomerId",
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
                name: "IX_asp_net_user_roles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_user_logins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_user_claims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_asp_net_role_claims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

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
                name: "PK_Parts",
                table: "Parts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "AppointmentId");

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
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

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
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
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
        }
    }
}
