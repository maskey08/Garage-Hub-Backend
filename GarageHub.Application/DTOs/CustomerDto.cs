namespace GarageHub.Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
        public decimal CreditBalance { get; set; }
        public List<VehicleDto> Vehicles { get; set; } = new();
        public List<PurchaseSummaryDto> Purchases { get; set; } = new();
        public int LoyaltyPoints { get; set; }
    }

    public class CreateCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? VehicleNumber { get; set; }
        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public int? VehicleYear { get; set; }
        public string? VehicleVin { get; set; }
    }

    public class VehicleDto
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class PurchaseSummaryDto
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public List<PurchaseItemDto> Items { get; set; } = new();
    }

    public class PurchaseItemDto
    {
        public int PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CustomerSearchRequest
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string SearchBy { get; set; } = string.Empty;
    }

    public class CustomerDashboardDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalVehicles { get; set; }
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalPurchases { get; set; }
        public int PendingPartRequests { get; set; }
        public List<VehicleDto> Vehicles { get; set; } = new();
        public List<AppointmentSummaryDto> RecentAppointments { get; set; } = new();
        public List<PurchaseSummaryDto> RecentPurchases { get; set; } = new();
    }

    public class AppointmentSummaryDto
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
