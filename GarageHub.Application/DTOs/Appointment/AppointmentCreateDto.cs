using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Application.DTOs.Appointment;

public class AppointmentCreateDto
{
    public int VehicleId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string? Notes { get; set; }
}