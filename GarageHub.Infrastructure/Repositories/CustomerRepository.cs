using Microsoft.EntityFrameworkCore;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;

namespace GarageHub.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer?> GetCustomerWithDetailsAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Vehicles)
            .Include(c => c.Purchases)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, string searchBy)
    {
        var query = _context.Customers
            .Include(c => c.Vehicles)
            .AsQueryable();
        
        switch (searchBy.ToLower())
        {
            case "name":
                query = query.Where(c => c.FullName.Contains(searchTerm));
                break;
            case "phone":
                query = query.Where(c => c.Phone.Contains(searchTerm));
                break;
            case "id":
                if (int.TryParse(searchTerm, out int id))
                    query = query.Where(c => c.Id == id);
                break;
            case "vehiclenumber":
                query = query.Where(c => c.Vehicles.Any(v => v.VehicleNumber.Contains(searchTerm)));
                break;
            default:
                return new List<Customer>();
        }
        
        return await query.ToListAsync();
    }

    public async Task<Customer?> GetCustomerByUserIdAsync(int userId)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
    }
}