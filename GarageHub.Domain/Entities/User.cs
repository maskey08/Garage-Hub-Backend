namespace GarageHub.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHashText { get; set; } = string.Empty;
    public string Role { get; set; } = "customer";
    public decimal TotalSpent { get; set; }
    public decimal CreditBalance { get; set; }
    public DateOnly? CreditDueDate { get; set; }
    public int? ManagedBy { get; set; }
    public int LoyaltyPoints { get; set; } = 0;
    public DateTime CreatedAt { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<PartRequest> PartRequests { get; set; } = [];
    public ICollection<SalesInvoice> SalesInvoices { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];

    public User()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
