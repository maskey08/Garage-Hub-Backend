using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GarageHub.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;
    private readonly UserManager<User> _userManager;

    public CustomerService(AppDbContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<User?> GetProfileAsync(int userId)
        => await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
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

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
    {
        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already registered");

        // Create new user for customer
        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        // Use the password provided by staff
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create customer account: {errors}");
        }

        // Assign customer role
        await _userManager.AddToRoleAsync(user, "customer");

        // Return customer DTO
        return new CustomerDto
        {
            Id = user.Id,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Phone = user.Phone,
            Email = user.Email ?? string.Empty,
            Address = dto.Address ?? string.Empty,
            RegisteredDate = user.CreatedAt,
            CreditBalance = 0,
            Vehicles = new(),
            Purchases = new()
        };
    }

    private string GenerateTemporaryPassword()
    {
        // Generate a temporary password that meets complexity requirements
        const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*";

        var random = new Random();
        var password = new StringBuilder();

        // Add at least one character from each category
        password.Append(upperChars[random.Next(upperChars.Length)]);
        password.Append(lowerChars[random.Next(lowerChars.Length)]);
        password.Append(digits[random.Next(digits.Length)]);
        password.Append(specialChars[random.Next(specialChars.Length)]);

        // Fill the rest randomly
        var allChars = upperChars + lowerChars + digits + specialChars;
        for (int i = 0; i < 4; i++)
        {
            password.Append(allChars[random.Next(allChars.Length)]);
        }

        // Shuffle the password
        var passwordArray = password.ToString().ToCharArray();
        for (int i = passwordArray.Length - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            (passwordArray[i], passwordArray[randomIndex]) = (passwordArray[randomIndex], passwordArray[i]);
        }

        return new string(passwordArray);
    }
}
