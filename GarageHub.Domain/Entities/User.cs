
namespace GarageHub.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "customer";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<PartRequest> PartRequests { get; set; } = [];
    public ICollection<SalesInvoice> SalesInvoices { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}