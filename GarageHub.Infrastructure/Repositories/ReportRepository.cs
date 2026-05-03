using Microsoft.EntityFrameworkCore;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;

namespace GarageHub.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Purchase>()
                .Where(p => p.PurchaseDate >= startDate && p.PurchaseDate <= endDate)
                .Include(p => p.Customer)
                .ToListAsync();
        }

        public async Task<List<Part>> GetLowStockPartsAsync(int threshold = 10)
        {
            return await _context.Set<Part>()
                .Where(p => p.StockQuantity < threshold)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetTopCustomersAsync(int limit = 10)
        {
            return await _context.Set<Customer>()
                .Include(c => c.Purchases)
                .OrderByDescending(c => c.Purchases.Sum(p => p.TotalAmount))
                .Take(limit)
                .ToListAsync();
        }
    }
}