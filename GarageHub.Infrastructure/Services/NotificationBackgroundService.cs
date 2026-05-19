using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Domain.Enums;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GarageHub.Infrastructure.Services;

public class NotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NotificationBackgroundService> _logger;

    public NotificationBackgroundService(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<NotificationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendLowStockNotificationsAsync(stoppingToken);
                await SendCreditRemindersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Automated notification cycle failed.");
            }

            var intervalHours = int.TryParse(_configuration["Automation:NotificationIntervalHours"], out var configured)
                ? Math.Max(configured, 1)
                : 24;

            await Task.Delay(TimeSpan.FromHours(intervalHours), stoppingToken);
        }
    }

    private async Task SendLowStockNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var adminEmail = _configuration["Automation:AdminAlertEmail"];
        if (string.IsNullOrWhiteSpace(adminEmail))
        {
            return;
        }

        var threshold = int.TryParse(_configuration["Automation:LowStockThreshold"], out var configuredThreshold)
            ? configuredThreshold
            : 10;

        var lowStockParts = await db.Parts
            .Where(part => part.StockQuantity <= threshold)
            .OrderBy(part => part.StockQuantity)
            .ToListAsync(cancellationToken);

        if (lowStockParts.Count == 0)
        {
            return;
        }

        await CreateStaffNotificationsAsync(db, "Low stock alert", $"{lowStockParts.Count} part(s) are low on stock.", NotificationType.LowStock, cancellationToken);

        var rows = string.Join("", lowStockParts.Select(part =>
            $"<tr><td>{part.PartName}</td><td>{part.PartNumber}</td><td>{part.StockQuantity}</td><td>{threshold}</td></tr>"));

        await emailService.SendInvoiceEmailAsync(
            adminEmail,
            "GarageHub Low Stock Alert",
            $"<h2>Low Stock Parts</h2><table border='1' cellpadding='8' cellspacing='0'><tr><th>Part</th><th>SKU</th><th>Stock</th><th>Threshold</th></tr>{rows}</table>");
    }

    private async Task SendCreditRemindersAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var cutoff = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));
        var customersWithCredit = await db.Users
            .Where(user => user.Role == "customer" &&
                           user.CreditBalance > 10000 &&
                           user.Email != "" &&
                           (user.CreditDueDate == null || user.CreditDueDate <= cutoff))
            .ToListAsync(cancellationToken);

        if (customersWithCredit.Count > 0)
        {
            await CreateStaffNotificationsAsync(db, "Credit reminder", $"{customersWithCredit.Count} customer(s) have credit above 10k for over a week.", NotificationType.GeneralAlert, cancellationToken);
        }

        foreach (var customer in customersWithCredit)
        {
            var dueText = customer.CreditDueDate.HasValue
                ? $" Due date: {customer.CreditDueDate.Value:yyyy-MM-dd}."
                : string.Empty;

            await emailService.SendInvoiceEmailAsync(
                customer.Email,
                "GarageHub Credit Reminder",
                $"<p>Hello {customer.FirstName},</p><p>Your current credit balance is <strong>{customer.CreditBalance:N2}</strong>.{dueText}</p><p>Please clear your balance to keep your service account in good standing.</p>");
        }
    }

    private static async Task CreateStaffNotificationsAsync(
        AppDbContext db,
        string title,
        string message,
        NotificationType type,
        CancellationToken cancellationToken)
    {
        var since = DateTime.UtcNow.AddHours(-12);
        var users = await db.Users
            .Where(u => u.Role == "admin" || u.Role == "staff")
            .Select(u => u.UserId)
            .ToListAsync(cancellationToken);

        foreach (var userId in users)
        {
            var exists = await db.Notifications.AnyAsync(n =>
                n.UserId == userId && n.Title == title && n.Type == type && n.CreatedAt >= since,
                cancellationToken);

            if (!exists)
            {
                db.Notifications.Add(new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = type,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
