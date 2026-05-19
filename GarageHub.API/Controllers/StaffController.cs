using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;
    private readonly AppDbContext _db;

    public StaffController(IStaffService staffService, AppDbContext db)
    {
        _staffService = staffService;
        _db = db;
    }

    [HttpGet("dashboard")]
    [Authorize(Roles = "admin,staff")]
    public async Task<IActionResult> GetDashboard()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        var todaySales = await _db.SalesInvoices
            .Where(s => s.SaleDate >= today && s.SaleDate < tomorrow)
            .SumAsync(s => s.TotalAmount);
        var pendingInvoices = await _db.SalesInvoices.CountAsync(s => s.PaymentStatus == "pending");
        var newCustomers = await _db.Users.CountAsync(u => u.Role == "customer" && u.CreatedAt >= today);
        var activeAppointments = await _db.Appointments.CountAsync(a => a.Status != "cancelled" && a.ScheduledAt >= today);

        return Ok(new { todaySales, pendingInvoices, newCustomers, activeAppointments });
    }

    [HttpGet("recent-transactions")]
    [Authorize(Roles = "admin,staff")]
    public async Task<IActionResult> GetRecentTransactions()
    {
        var transactions = await _db.SalesInvoices
            .Include(s => s.Customer)
            .Include(s => s.Items)
            .ThenInclude(i => i.Part)
            .OrderByDescending(s => s.SaleDate)
            .Take(10)
            .Select(s => new
            {
                id = s.SaleId.ToString(),
                invoiceId = "INV-" + s.SaleId,
                customer = (s.Customer.FirstName + " " + s.Customer.LastName).Trim(),
                partSold = s.Items.Select(i => i.Part.PartName).FirstOrDefault() ?? "Parts sale",
                amount = s.TotalAmount,
                time = s.SaleDate.ToString("yyyy-MM-dd HH:mm"),
                status = s.PaymentStatus == "paid" ? "completed" : s.PaymentStatus
            })
            .ToListAsync();

        return Ok(transactions);
    }

    /// <summary>
    /// Get all staff members (admin and staff can access)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "admin,staff")]
    public async Task<IActionResult> GetAllStaff()
    {
        try
        {
            var staff = await _staffService.GetAllStaffAsync();
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Search staff by name, email, or phone
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "admin,staff")]
    public async Task<IActionResult> SearchStaff([FromQuery] string searchTerm)
    {
        try
        {
            var staff = await _staffService.SearchStaffAsync(searchTerm);
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Add a new staff member (admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddStaff([FromBody] AddStaffDto dto)
    {
        try
        {
            var result = await _staffService.AddStaffAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update staff member (admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateStaff(int id, [FromBody] UpdateStaffDto dto)
    {
        try
        {
            var result = await _staffService.UpdateStaffAsync(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete staff member (admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            await _staffService.DeleteStaffAsync(id);
            return Ok(new { message = "Staff deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
