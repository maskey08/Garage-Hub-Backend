using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingPartColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Parts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Parts");
        }
    }
}
