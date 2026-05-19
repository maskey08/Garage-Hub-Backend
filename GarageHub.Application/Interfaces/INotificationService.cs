using GarageHub.Application.DTOs.Notification;

namespace GarageHub.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync();
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteNotificationAsync(int id);
    }
}
