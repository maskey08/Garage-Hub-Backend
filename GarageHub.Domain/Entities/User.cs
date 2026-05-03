using Microsoft.AspNetCore.Identity;

namespace GarageHub.Domain.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<PartRequest> PartRequests { get; set; } = [];
    public ICollection<SalesInvoice> SalesInvoices { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}