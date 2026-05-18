using GarageHub.Application.DTOs.Notification;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notificationRepository;

        public NotificationService(IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Where(n => !n.IsRead).Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type
            };
            await _notificationRepository.AddAsync(notification);
            return new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return false;
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return false;
            await _notificationRepository.DeleteAsync(notification);
            return true;
        }
    }
}