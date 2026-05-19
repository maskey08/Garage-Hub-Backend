using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,staff")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly AppDbContext _db;

        public ReportsController(IReportService reportService, AppDbContext db)
        {
            _reportService = reportService;
            _db = db;
        }

        // GET: api/reports/daily?date=2026-04-30
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date)
        {
            var report = await _reportService.GetDailyReportAsync(date);
            return Ok(report);
        }

        // GET: api/reports/financial?period=Daily
        [HttpGet("financial")]
        public async Task<IActionResult> GetFinancialReport([FromQuery] string period, [FromQuery] DateTime? date, [FromQuery] int? year, [FromQuery] int? month)
        {
            if (string.IsNullOrWhiteSpace(period))
            {
                return BadRequest("Period is required.");
            }

            switch (period.Trim().ToLowerInvariant())
            {
                case "daily":
                    if (date == null)
                        return BadRequest("Date is required for daily reports.");
                    return Ok(await _reportService.GetDailyReportAsync(date.Value));
                case "monthly":
                    if (year == null || month == null)
                        return BadRequest("Year and month are required for monthly reports.");
                    return Ok(await _reportService.GetMonthlyReportAsync(year.Value, month.Value));
                case "yearly":
                    if (year == null)
                        return BadRequest("Year is required for yearly reports.");
                    return Ok(await _reportService.GetYearlyReportAsync(year.Value));
                default:
                    return BadRequest("Invalid period. Use Daily, Monthly, or Yearly.");
            }
        }

        // GET: api/reports/monthly?year=2026&month=4
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12");

            var report = await _reportService.GetMonthlyReportAsync(year, month);
            return Ok(report);
        }

        // GET: api/reports/yearly?year=2026
        [HttpGet("yearly")]
        public async Task<IActionResult> GetYearlyReport([FromQuery] int year)
        {
            var report = await _reportService.GetYearlyReportAsync(year);
            return Ok(report);
        }

        // GET: api/reports/low-stock?threshold=10
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockAlerts([FromQuery] int threshold = 10)
        {
            var alerts = await _reportService.GetLowStockAlertsAsync(threshold);
            return Ok(alerts);
        }

        // GET: api/reports/inventory?threshold=10
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventoryReport([FromQuery] int threshold = 10)
        {
            var parts = await _db.Parts.ToListAsync();
            var total = parts.Count;
            var colors = new[] { "#0059bb", "#405e96", "#717786", "#16a34a", "#dc2626", "#9333ea" };
            var distribution = parts
                .GroupBy(p => string.IsNullOrWhiteSpace(p.Category) ? "Uncategorized" : p.Category)
                .Select((group, index) => new
                {
                    category = group.Key,
                    count = group.Count(),
                    percentage = total == 0 ? 0 : Math.Round(group.Count() * 100m / total, 2),
                    color = colors[index % colors.Length]
                })
                .ToList();

            return Ok(new { totalSkus = total, distribution });
        }

        // GET: api/reports/top-customers?limit=10
        [HttpGet("top-customers")]
        public async Task<IActionResult> GetTopCustomers([FromQuery] int limit = 10)
        {
            var customers = await _reportService.GetTopCustomersAsync(limit);
            return Ok(customers.Select(c => new
            {
                customerId = c.CustomerId,
                name = c.CustomerName,
                totalPurchases = c.TotalPurchases,
                totalSpent = c.TotalSpent,
                loyaltyStatus = c.TotalSpent >= 50000 ? "Platinum" : c.TotalSpent >= 20000 ? "Gold" : "Silver",
                pendingCredit = 0
            }));
        }
    }
}
