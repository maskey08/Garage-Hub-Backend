using GarageHub.Domain.Entities;

namespace GarageHub.Infrastructure.Repositories
{
    public interface IReportRepository
    {
        Task<List<Purchase>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Part>> GetLowStockPartsAsync(int threshold = 10);
        Task<List<Customer>> GetTopCustomersAsync(int limit = 10);
    }
}