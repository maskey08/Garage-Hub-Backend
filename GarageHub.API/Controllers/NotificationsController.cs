using GarageHub.Domain.Entities;
using GarageHub.Domain.Enums;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public NotificationsController(AppDbContext db)
    {
        _db = db;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
    private string Role => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        await EnsureLiveAlertsAsync();

        var query = _db.Notifications.AsQueryable();
        if (Role != "admin" && Role != "staff")
        {
            query = query.Where(n => n.UserId == UserId);
        }

        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(25)
            .Select(n => new
            {
                id = n.NotificationId,
                title = n.Title,
                message = n.Message,
                type = n.Type.ToString(),
                isRead = n.IsRead,
                createdAt = n.CreatedAt
            })
            .ToListAsync();

        return Ok(notifications);
    }

    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var notification = await _db.Notifications.FirstOrDefaultAsync(n => n.NotificationId == id);
        if (notification == null)
            return NotFound();

        notification.IsRead = true;
        await _db.SaveChangesAsync();
        return Ok();
    }

    private async Task EnsureLiveAlertsAsync()
    {
        var recipientIds = await _db.Users
            .Where(u => u.Role == "admin" || u.Role == "staff")
            .Select(u => u.UserId)
            .ToListAsync();

        var lowStockCount = await _db.Parts.CountAsync(p => p.StockQuantity <= 10);
        if (lowStockCount > 0)
        {
            foreach (var userId in recipientIds)
            {
                await AddIfMissingAsync(userId, "Low stock alert", $"{lowStockCount} part(s) are at or below reorder stock.", NotificationType.LowStock);
            }
        }

        var cutoff = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));
        var overdueCreditCount = await _db.Users.CountAsync(u =>
            u.Role == "customer" &&
            u.CreditBalance > 10000 &&
            (u.CreditDueDate == null || u.CreditDueDate <= cutoff));

        if (overdueCreditCount > 0)
        {
            foreach (var userId in recipientIds)
            {
                await AddIfMissingAsync(userId, "Credit reminder", $"{overdueCreditCount} customer(s) have credit above 10k for over a week.", NotificationType.GeneralAlert);
            }
        }
    }

    private async Task AddIfMissingAsync(int userId, string title, string message, NotificationType type)
    {
        var since = DateTime.UtcNow.AddHours(-12);
        var exists = await _db.Notifications.AnyAsync(n =>
            n.UserId == userId && n.Title == title && n.Type == type && n.CreatedAt >= since);

        if (exists)
            return;

        _db.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
    }
}
