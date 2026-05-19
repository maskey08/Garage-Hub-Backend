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
        var query = _db.Notifications.Where(n => n.UserId == UserId);

        var rawNotifications = await query
            .OrderByDescending(n => n.CreatedAt)
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

        var notifications = rawNotifications
            .GroupBy(n => new { n.title, n.message, n.type })
            .Select(group => group.First())
            .OrderByDescending(n => n.createdAt)
            .Take(25)
            .ToList();

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

}
