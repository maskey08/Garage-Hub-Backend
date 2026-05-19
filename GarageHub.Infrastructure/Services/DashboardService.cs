using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _dbContext;

    public DashboardService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardStatisticsDto> GetStatisticsAsync()
    {
        var totalCustomersCount = await _dbContext.Users
            .CountAsync(u => u.Role == "customer" || string.IsNullOrEmpty(u.Role));

        var totalStaffCount = await _dbContext.Users
            .CountAsync(u => u.Role == "staff" || u.Role == "admin");

        var totalAppointmentsCount = await _dbContext.Appointments.CountAsync();
        var pendingAppointmentsCount = await _dbContext.Appointments
            .CountAsync(a => a.Status == "pending");
        var completedAppointmentsCount = await _dbContext.Appointments
            .CountAsync(a => a.Status == "completed");

        var totalRevenueAmount = await _dbContext.SalesInvoices
            .SumAsync(si => (decimal)si.TotalAmount);

        var totalSalesCount = await _dbContext.SalesInvoices.CountAsync();

        var totalPartRequestsCount = await _dbContext.PartRequests.CountAsync();
        var pendingPartRequestsCount = await _dbContext.PartRequests
            .CountAsync(pr => pr.Status == "pending");

        return new DashboardStatisticsDto
        {
            TotalCustomers = totalCustomersCount,
            TotalStaff = totalStaffCount,
            TotalAppointments = totalAppointmentsCount,
            PendingAppointments = pendingAppointmentsCount,
            CompletedAppointments = completedAppointmentsCount,
            TotalRevenue = totalRevenueAmount,
            TotalSales = totalSalesCount,
            TotalPartRequests = totalPartRequestsCount,
            PendingPartRequests = pendingPartRequestsCount
        };
    }

    public async Task<List<TransactionDto>> GetRecentActivityAsync(int limit = 6)
    {
        return await _dbContext.SalesInvoices
            .OrderByDescending(si => si.SaleDate)
            .Take(limit)
            .Select(si => new TransactionDto
            {
                Id = si.SaleId,
                InvoiceId = $"INV-{si.SaleId:D5}",
                Customer = si.Customer.Email ?? "Customer",
                PartSold = si.Items.Select(i => i.Part.PartName).FirstOrDefault() ?? "Service",
                Amount = (decimal)si.TotalAmount,
                Time = si.SaleDate,
                Status = si.PaymentStatus
            })
            .ToListAsync();
    }
}
