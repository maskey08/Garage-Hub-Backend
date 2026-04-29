using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities;

public class Appointment
{
    public int AppointmentId { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public int? StaffId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Status { get; set; } = "pending"; // pending | confirmed | completed | cancelled
    public string? Notes { get; set; }

    public User Customer { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public Review? Review { get; set; }
}
