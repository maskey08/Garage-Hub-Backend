using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Repositories;

namespace GarageHub.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<DailyReportDto> GetDailyReportAsync(DateTime date)
        {
            var startDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            var endDate = startDate.AddDays(1).AddSeconds(-1);

            var sales = await _reportRepository.GetPurchasesByDateRangeAsync(startDate, endDate);

            var totalRevenue = sales.Sum(s => (decimal)s.TotalAmount);
            var totalOrders = sales.Count;

            return new DailyReportDto
            {
                Date = date.Date,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0
            };
        }

        public async Task<MonthlyReportDto> GetMonthlyReportAsync(int year, int month)
        {
            var startDate = DateTime.SpecifyKind(new DateTime(year, month, 1), DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);

            var sales = await _reportRepository.GetPurchasesByDateRangeAsync(startDate, endDate);

            var totalRevenue = sales.Sum(s => (decimal)s.TotalAmount);
            var totalOrders = sales.Count;

            var dailyBreakdown = sales
                .GroupBy(s => DateTime.SpecifyKind(s.SaleDate.Date, DateTimeKind.Utc))
                .Select(g => new DailyReportDto
                {
                    Date = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(s => (decimal)s.TotalAmount),
                    AverageOrderValue = g.Count() > 0 ? g.Sum(s => (decimal)s.TotalAmount) / g.Count() : 0
                })
                .OrderBy(d => d.Date)
                .ToList();

            return new MonthlyReportDto
            {
                Year = year,
                Month = month,
                MonthName = new DateTime(year, month, 1).ToString("MMMM"),
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                DailyBreakdown = dailyBreakdown
            };
        }

        public async Task<YearlyReportDto> GetYearlyReportAsync(int year)
        {
            var startDate = DateTime.SpecifyKind(new DateTime(year, 1, 1), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(year, 12, 31, 23, 59, 59), DateTimeKind.Utc);

            var sales = await _reportRepository.GetPurchasesByDateRangeAsync(startDate, endDate);

            var totalRevenue = sales.Sum(s => (decimal)s.TotalAmount);
            var totalOrders = sales.Count;

            var monthlyBreakdown = Enumerable.Range(1, 12)
                .Select(month => new
                {
                    Month = month,
                    Sales = sales.Where(s => s.SaleDate.Month == month).ToList()
                })
                .Select(m => new MonthlyReportDto
                {
                    Year = year,
                    Month = m.Month,
                    MonthName = new DateTime(year, m.Month, 1).ToString("MMMM"),
                    TotalOrders = m.Sales.Count,
                    TotalRevenue = m.Sales.Sum(s => (decimal)s.TotalAmount),
                    DailyBreakdown = new List<DailyReportDto>()
                })
                .ToList();

            return new YearlyReportDto
            {
                Year = year,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                MonthlyBreakdown = monthlyBreakdown
            };
        }

        public async Task<List<LowStockAlertDto>> GetLowStockAlertsAsync(int threshold = 10)
        {
            var lowStockParts = await _reportRepository.GetLowStockPartsAsync(threshold);

            return lowStockParts.Select(p => new LowStockAlertDto
            {
                PartId = p.Id,
                PartName = p.PartName,
                Brand = p.Brand,
                CurrentStock = p.StockQuantity,
                Threshold = threshold
            }).ToList();
        }

        public async Task<List<TopCustomerDto>> GetTopCustomersAsync(int limit = 10)
        {
            var topCustomers = await _reportRepository.GetTopCustomersAsync(limit);

            return topCustomers.Select(c => new TopCustomerDto
            {
                CustomerId = c.UserId,
                CustomerName = $"{c.FirstName} {c.LastName}".Trim(),
                Phone = c.Phone,
                Email = c.Email,
                TotalPurchases = c.SalesInvoices?.Count ?? 0,
                TotalSpent = (decimal)(c.SalesInvoices?.Sum(s => s.TotalAmount) ?? 0)
            }).ToList();
        }
    }
}