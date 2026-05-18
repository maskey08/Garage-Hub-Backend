namespace GarageHub.Domain.Entities;

/// <summary>
/// Represents the user data in the custom 'users' table
/// </summary>
public class UserProfile
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string Role { get; set; } = "customer";
    public decimal TotalSpent { get; set; } = 0;
    public decimal CreditBalance { get; set; } = 0;
    public DateTime? CreditDueDate { get; set; }
    public int? ManagedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public User? User { get; set; }

    public UserProfile()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
