namespace GarageHub.Application.DTOs.Appointment;

public class AppointmentResponseDto
{
    public int AppointmentId { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public string VehicleInfo { get; set; } = string.Empty; // "Make Model (Number)"
    public DateTime ScheduledAt { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

