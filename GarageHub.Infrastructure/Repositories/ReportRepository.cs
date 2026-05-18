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

        public async Task<List<SalesInvoice>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SalesInvoices
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .Include(s => s.Customer)
                .ToListAsync();
        }

        public async Task<List<Part>> GetLowStockPartsAsync(int threshold = 10)
        {
            return await _context.Parts
                .Where(p => p.StockQuantity < threshold)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }

        public async Task<List<User>> GetTopCustomersAsync(int limit = 10)
        {
            return await _context.Users
                .Include(u => u.SalesInvoices)
                .OrderByDescending(u => u.SalesInvoices.Sum(s => s.TotalAmount))
                .Take(limit)
                .ToListAsync();
        }
    }
}