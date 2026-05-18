using GarageHub.Domain.Enums;

namespace GarageHub.Application.DTOs.Notification;

public class CreateNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
}