using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoyaltyPointsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_CustomerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Vehicles_VehicleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PartRequests_Users_CustomerId",
                table: "PartRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Appointments_AppointmentId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItems_SalesInvoices_SaleId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Users_CustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_UserId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

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
                name: "PK_PartRequests",
                table: "PartRequests");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "vehicles");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "reviews");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notifications");

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
                name: "PartRequests",
                newName: "part_requests");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_UserId",
                table: "vehicles",
                newName: "IX_vehicles_UserId");

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

            migrationBuilder.RenameIndex(
                name: "IX_PartRequests_CustomerId",
                table: "part_requests",
                newName: "IX_part_requests_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles",
                column: "VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reviews",
                table: "reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notifications",
                table: "notifications",
                column: "NotificationId");

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
                name: "PK_part_requests",
                table: "part_requests",
                column: "RequestId");

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
                name: "FK_sales_invoice_items_sales_invoices_SaleId",
                table: "sales_invoice_items",
                column: "SaleId",
                principalTable: "sales_invoices",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_invoices_users_CustomerId",
                table: "sales_invoices",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_notifications_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_part_requests_users_CustomerId",
                table: "part_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_appointments_AppointmentId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_CustomerId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoice_items_sales_invoices_SaleId",
                table: "sales_invoice_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_invoices_users_CustomerId",
                table: "sales_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_users_UserId",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reviews",
                table: "reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notifications",
                table: "notifications");

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
                name: "PK_part_requests",
                table: "part_requests");

            migrationBuilder.RenameTable(
                name: "vehicles",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "notifications",
                newName: "Notifications");

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
                name: "part_requests",
                newName: "PartRequests");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_UserId",
                table: "Vehicles",
                newName: "IX_Vehicles_UserId");

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
                name: "IX_part_requests_CustomerId",
                table: "PartRequests",
                newName: "IX_PartRequests_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationId");

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
                name: "PK_PartRequests",
                table: "PartRequests",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_CustomerId",
                table: "Appointments",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Vehicles_VehicleId",
                table: "Appointments",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartRequests_Users_CustomerId",
                table: "PartRequests",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Appointments_AppointmentId",
                table: "Reviews",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_SalesInvoices_SaleId",
                table: "SalesInvoiceItems",
                column: "SaleId",
                principalTable: "SalesInvoices",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Users_CustomerId",
                table: "SalesInvoices",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
