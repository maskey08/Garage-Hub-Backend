using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GarageHub.Application.Interfaces;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/reports/daily?date=2026-04-30
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date)
        {
            var report = await _reportService.GetDailyReportAsync(date);
            return Ok(report);
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

        // GET: api/reports/top-customers?limit=10
        [HttpGet("top-customers")]
        public async Task<IActionResult> GetTopCustomers([FromQuery] int limit = 10)
        {
            var customers = await _reportService.GetTopCustomersAsync(limit);
            return Ok(customers);
        }
    }
}