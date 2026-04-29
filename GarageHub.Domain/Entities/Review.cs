using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities;

public class Review
{
    public int ReviewId { get; set; }
    public int CustomerId { get; set; }
    public int AppointmentId { get; set; }
    public int Rating { get; set; } // 1–5
    public string? Comment { get; set; }
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

    public User Customer { get; set; } = null!;
    public Appointment Appointment { get; set; } = null!;
}
