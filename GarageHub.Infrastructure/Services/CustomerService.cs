using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db) => _db = db;

    public async Task<User?> GetProfileAsync(int userId)
        => await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.Phone = phone;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<SalesInvoice>> GetPurchaseHistoryAsync(int customerId)
        => await _db.SalesInvoices.Where(s => s.CustomerId == customerId).ToListAsync();

    public async Task<IEnumerable<Vehicle>> GetVehiclesAsync(int customerId)
        => await _db.Vehicles.Where(v => v.UserId == customerId).ToListAsync();

    public async Task<Vehicle> AddVehicleAsync(int customerId, Vehicle vehicle)
    {
        vehicle.UserId = customerId;
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();
        return vehicle;
    }
}
