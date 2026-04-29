using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities;

public class Notification
{
    public int NotificationId { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; } = string.Empty; // low_stock | credit_reminder
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}