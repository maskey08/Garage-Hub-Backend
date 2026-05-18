using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageHub.Domain.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public int user_id { get; set; }

    [Column("first_name")]
    public string first_name { get; set; } = string.Empty;

    [Column("last_name")]
    public string last_name { get; set; } = string.Empty;

    [Column("email")]
    public string email { get; set; } = string.Empty;

    [Column("phone")]
    public string phone { get; set; } = string.Empty;

    [Column("password_hash")]
    public string password_hash { get; set; } = string.Empty;

    [Column("role")]
    public string role { get; set; } = "customer";

    [Column("total_spent")]
    public decimal total_spent { get; set; }

    [Column("credit_balance")]
    public decimal credit_balance { get; set; }

    [Column("credit_due_date")]
    public DateTime? credit_due_date { get; set; }

    [Column("managed_by")]
    public int? managed_by { get; set; }

    [Column("created_at")]
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    // Navigation properties
    // public User? Manager { get; set; }  // COMMENTED OUT - causes mapping error
    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<PartRequest> PartRequests { get; set; } = [];
    public ICollection<SalesInvoice> SalesInvoices { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}