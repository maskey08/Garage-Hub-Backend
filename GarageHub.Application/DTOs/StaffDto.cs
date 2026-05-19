namespace GarageHub.Application.DTOs;

public class StaffDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = "staff";
    public string? SubRole { get; set; }
    public string Status { get; set; } = "active";
}