using GarageHub.Domain.Entities;

namespace GarageHub.Infrastructure.Repositories
{
    public interface IReportRepository
    {
        Task<List<SalesInvoice>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Part>> GetLowStockPartsAsync(int threshold = 10);
        Task<List<User>> GetTopCustomersAsync(int limit = 10);
    }
}