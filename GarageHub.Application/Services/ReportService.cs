using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Repositories;

namespace GarageHub.Application.Services
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
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);

            var purchases = await _reportRepository.GetPurchasesByDateRangeAsync(startDate, endDate);

            var totalRevenue = purchases.Sum(p => p.TotalAmount);
            var totalOrders = purchases.Count;

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
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);

            var purchases = await _reportRepository.GetPurchasesByDateRangeAsync(startDate, endDate);

            var totalRevenue = purchases.Sum(p => p.TotalAmount);
            var totalOrders = purchases.Count;

            var dailyBreakdown = purchases
                .GroupBy(p => p.PurchaseDate.Date)
                .Select(g => new DailyReportDto
                {
                    Date = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(p => p.TotalAmount),
                    AverageOrderValue = g.Sum(p => p.TotalAmount) / g.Count()
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
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31, 23, 59, 59);

            var purchases = await _reportRepository.GetPurchasesByDateRangeAsync(startDate, endDate);

            var totalRevenue = purchases.Sum(p => p.TotalAmount);
            var totalOrders = purchases.Count;

            var monthlyBreakdown = Enumerable.Range(1, 12)
                .Select(month => new
                {
                    Month = month,
                    Purchases = purchases.Where(p => p.PurchaseDate.Month == month).ToList()
                })
                .Select(m => new MonthlyReportDto
                {
                    Year = year,
                    Month = m.Month,
                    MonthName = new DateTime(year, m.Month, 1).ToString("MMMM"),
                    TotalOrders = m.Purchases.Count,
                    TotalRevenue = m.Purchases.Sum(p => p.TotalAmount),
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
                CustomerId = c.Id,
                CustomerName = c.FullName,
                Phone = c.Phone,
                Email = c.Email,
                TotalPurchases = c.Purchases?.Count ?? 0,
                TotalSpent = c.Purchases?.Sum(p => p.TotalAmount) ?? 0
            }).ToList();
        }
    }
}