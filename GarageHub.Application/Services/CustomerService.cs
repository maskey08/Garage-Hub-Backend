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
    private readonly IUserProfileService _userProfileService;

    public CustomerService(AppDbContext db, UserManager<User> userManager, IUserProfileService userProfileService)
    {
        _db = db;
        _userManager = userManager;
        _userProfileService = userProfileService;
    }

    public async Task<User?> GetProfileAsync(int userId)
        => await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Phone = phone;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<SalesInvoice>> GetPurchaseHistoryAsync(int customerId)
        => await _db.SalesInvoices.Where(s => s.CustomerId == customerId).ToListAsync();

    public async Task<IEnumerable<VehicleResponseDto>> GetVehiclesAsync(int customerId)
    {
        var vehicles = await _db.Vehicles.Where(v => v.UserId == customerId).ToListAsync();
        return vehicles.Select(v => new VehicleResponseDto
        {
            VehicleId = v.VehicleId,
            VehicleNumber = v.VehicleNumber,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            Vin = v.Vin
        });
    }

    public async Task<VehicleResponseDto> AddVehicleAsync(int customerId, AddVehicleDto vehicleDto)
    {
        var vehicle = new Vehicle
        {
            UserId = customerId,
            VehicleNumber = vehicleDto.VehicleNumber,
            Make = vehicleDto.Make,
            Model = vehicleDto.Model,
            Year = vehicleDto.Year,
            Vin = vehicleDto.Vin ?? string.Empty
        };
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();

        return new VehicleResponseDto
        {
            VehicleId = vehicle.VehicleId,
            VehicleNumber = vehicle.VehicleNumber,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Vin = vehicle.Vin
        };
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

        // Sync user data to the custom 'users' table
        await _userProfileService.CreateUserProfileAsync(user, "customer");

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

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _db.Users
            .Where(u => _db.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                    _db.Roles.Any(r => r.Id == ur.RoleId && r.Name == "customer")))
            .ToListAsync();

        return customers.Select(u => new CustomerDto
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? string.Empty,
            Phone = u.Phone ?? string.Empty,
            Address = string.Empty,
            RegisteredDate = u.CreatedAt,
            CreditBalance = 0,
            Vehicles = new(),
            Purchases = new()
        });
    }

    public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllCustomersAsync();

        var searchLower = searchTerm.ToLower();
        var customers = await _db.Users
            .Where(u => _db.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                    _db.Roles.Any(r => r.Id == ur.RoleId && r.Name == "customer"))
                && (u.FirstName.ToLower().Contains(searchLower) ||
                    u.LastName.ToLower().Contains(searchLower) ||
                    u.Email!.ToLower().Contains(searchLower) ||
                    u.Phone.ToLower().Contains(searchLower)))
            .ToListAsync();

        return customers.Select(u => new CustomerDto
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? string.Empty,
            Phone = u.Phone ?? string.Empty,
            Address = string.Empty,
            RegisteredDate = u.CreatedAt,
            CreditBalance = 0,
            Vehicles = new(),
            Purchases = new()
        });
    }

    public async Task<CustomerDashboardDto> GetCustomerDashboardAsync(int customerId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == customerId);
        if (user == null)
            throw new KeyNotFoundException("Customer not found");

        var vehicles = await _db.Vehicles.Where(v => v.UserId == customerId).ToListAsync();
        var appointments = await _db.Appointments.Where(a => a.CustomerId == customerId).ToListAsync();
        var purchases = await _db.SalesInvoices.Where(s => s.CustomerId == customerId).ToListAsync();
        var partRequests = await _db.PartRequests.Where(pr => pr.CustomerId == customerId).ToListAsync();

        var totalAppointments = appointments.Count;
        var pendingAppointments = appointments.Count(a => a.Status?.ToLower() == "pending");
        var completedAppointments = appointments.Count(a => a.Status?.ToLower() == "completed");
        var totalSpent = purchases.Sum(p => p.TotalAmount);
        var pendingPartRequests = partRequests.Count(pr => pr.Status?.ToLower() == "pending");

        return new CustomerDashboardDto
        {
            UserId = customerId,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? string.Empty,
            TotalVehicles = vehicles.Count,
            TotalAppointments = totalAppointments,
            PendingAppointments = pendingAppointments,
            CompletedAppointments = completedAppointments,
            TotalSpent = (decimal)totalSpent,
            TotalPurchases = purchases.Count,
            PendingPartRequests = pendingPartRequests,
            Vehicles = vehicles.Select(v => new VehicleDto
            {
                Id = v.VehicleId,
                VehicleNumber = v.VehicleNumber,
                Brand = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = string.Empty
            }).ToList(),
            RecentAppointments = appointments
                .OrderByDescending(a => a.ScheduledAt)
                .Take(5)
                .Select(a => new AppointmentSummaryDto
                {
                    Id = a.AppointmentId,
                    ScheduledDate = a.ScheduledAt,
                    Status = a.Status ?? "unknown",
                    ServiceType = a.ServiceType ?? string.Empty,
                    Notes = a.Notes ?? string.Empty
                }).ToList(),
            RecentPurchases = purchases
                .OrderByDescending(p => p.SaleDate)
                .Take(5)
                .Select(p => new PurchaseSummaryDto
                {
                    Id = p.SaleId,
                    PurchaseDate = p.SaleDate,
                    TotalAmount = (decimal)p.TotalAmount,
                    IsPaid = p.PaymentStatus?.ToLower() == "paid"
                }).ToList()
        };
    }

    public async Task<IEnumerable<Appointment>> GetServiceHistoryAsync(int customerId)
        => await _db.Appointments
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.ScheduledAt)
            .ToListAsync();
}