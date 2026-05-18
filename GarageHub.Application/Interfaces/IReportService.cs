using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces
{
    public interface IReportService
    {
        Task<DailyReportDto> GetDailyReportAsync(DateTime date);
        Task<MonthlyReportDto> GetMonthlyReportAsync(int year, int month);
        Task<YearlyReportDto> GetYearlyReportAsync(int year);
        Task<List<LowStockAlertDto>> GetLowStockAlertsAsync(int threshold = 10);
        Task<List<TopCustomerDto>> GetTopCustomersAsync(int limit = 10);
    }
}