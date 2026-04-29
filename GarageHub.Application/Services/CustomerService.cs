using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;
    public CustomerService(AppDbContext db) => _db = db;

    public async Task<User?> GetProfileAsync(int userId)
        => await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new Exception("User not found.");
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Phone = phone;
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<SalesInvoice>> GetPurchaseHistoryAsync(int customerId)
        => await _db.SalesInvoices
            .Include(s => s.Items)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

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