using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GarageHub.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;
    private readonly IUserProfileService _userProfileService;

    public CustomerService(AppDbContext db, IUserProfileService userProfileService)
    {
        _db = db;
        _userProfileService = userProfileService;
    }

    public async Task<User?> GetProfileAsync(int userId)
        => await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "customer");

    public async Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "customer");
        if (user == null)
            throw new KeyNotFoundException("Customer not found");

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

    public async Task<CustomerDto> GetCustomerDetailsAsync(int customerId)
    {
        var user = await _db.Users
            .Include(u => u.Vehicles)
            .FirstOrDefaultAsync(u => u.UserId == customerId && u.Role == "customer");

        if (user == null)
            throw new KeyNotFoundException("Customer not found");

        return new CustomerDto
        {
            Id = user.UserId,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? string.Empty,
            Phone = user.Phone ?? string.Empty,
            Address = string.Empty,
            RegisteredDate = user.CreatedAt,
            CreditBalance = user.CreditBalance,
            LoyaltyPoints = user.LoyaltyPoints,
            Vehicles = user.Vehicles.Select(v => new VehicleDto
            {
                Id = v.VehicleId,
                VehicleNumber = v.VehicleNumber,
                Brand = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = string.Empty
            }).ToList(),
            Purchases = new()
        };
    }

    public async Task<IEnumerable<PurchaseSummaryDto>> GetCustomerPurchaseHistoryAsync(int customerId)
    {
        var purchases = await _db.SalesInvoices
            .Include(s => s.Items)
            .ThenInclude(i => i.Part)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

        return purchases.Select(p => new PurchaseSummaryDto
        {
            Id = p.SaleId,
            PurchaseDate = p.SaleDate,
            TotalAmount = (decimal)p.TotalAmount,
            IsPaid = p.PaymentStatus?.ToLower() == "paid",
            InvoiceNumber = $"INV-{p.SaleId}",
            Items = p.Items.Select(i => new PurchaseItemDto
            {
                PartId = i.PartId,
                PartName = i.Part.PartName,
                Quantity = i.Quantity,
                UnitPrice = (decimal)i.UnitPrice,
                TotalPrice = (decimal)i.TotalPrice
            }).ToList()
        });
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
    {
        // Check if email already exists
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already registered");

        // Create new user for customer
        var user = new User
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Role = "customer",
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHashText = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Sync user data to the custom 'users' table
        await _userProfileService.CreateUserProfileAsync(user, "customer");

        if (!string.IsNullOrWhiteSpace(dto.VehicleNumber))
        {
            var vehicle = new Vehicle
            {
                UserId = user.UserId,
                VehicleNumber = dto.VehicleNumber,
                Make = dto.VehicleMake ?? string.Empty,
                Model = dto.VehicleModel ?? string.Empty,
                Year = dto.VehicleYear ?? DateTime.UtcNow.Year,
                Vin = dto.VehicleVin ?? string.Empty
            };
            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();
        }

        // Return customer DTO
        return new CustomerDto
        {
            Id = user.UserId,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Phone = user.Phone,
            Email = user.Email ?? string.Empty,
            Address = dto.Address ?? string.Empty,
            RegisteredDate = user.CreatedAt,
            CreditBalance = 0,
            LoyaltyPoints = user.LoyaltyPoints,
            Vehicles = string.IsNullOrWhiteSpace(dto.VehicleNumber)
                ? new()
                : new List<VehicleDto>
                {
                    new()
                    {
                        Id = 0,
                        VehicleNumber = dto.VehicleNumber,
                        Brand = dto.VehicleMake ?? string.Empty,
                        Model = dto.VehicleModel ?? string.Empty,
                        Year = dto.VehicleYear ?? DateTime.UtcNow.Year,
                        Color = string.Empty
                    }
                },
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
            .Include(u => u.Vehicles)
            .Where(u => u.Role == "customer")
            .ToListAsync();

        return customers.Select(u => new CustomerDto
        {
            Id = u.UserId,
            FullName = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? string.Empty,
            Phone = u.Phone ?? string.Empty,
            Address = string.Empty,
            RegisteredDate = u.CreatedAt,
            CreditBalance = u.CreditBalance,
            LoyaltyPoints = u.LoyaltyPoints,
            Vehicles = u.Vehicles.Select(v => new VehicleDto
            {
                Id = v.VehicleId,
                VehicleNumber = v.VehicleNumber,
                Brand = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = string.Empty
            }).ToList(),
            Purchases = new()
        });
    }

    public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllCustomersAsync();

        var searchLower = searchTerm.ToLower();
        var customers = await _db.Users
            .Include(u => u.Vehicles)
            .Where(u => u.Role == "customer"
                && (u.FirstName.ToLower().Contains(searchLower) ||
                    u.LastName.ToLower().Contains(searchLower) ||
                    u.Email!.ToLower().Contains(searchLower) ||
                    u.Phone.ToLower().Contains(searchLower) ||
                    u.UserId.ToString().Contains(searchLower) ||
                    u.Vehicles.Any(v => v.VehicleNumber.ToLower().Contains(searchLower))))
            .ToListAsync();

        return customers.Select(u => new CustomerDto
        {
            Id = u.UserId,
            FullName = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? string.Empty,
            Phone = u.Phone ?? string.Empty,
            Address = string.Empty,
            RegisteredDate = u.CreatedAt,
            CreditBalance = u.CreditBalance,
            LoyaltyPoints = u.LoyaltyPoints,
            Vehicles = u.Vehicles.Select(v => new VehicleDto
            {
                Id = v.VehicleId,
                VehicleNumber = v.VehicleNumber,
                Brand = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = string.Empty
            }).ToList(),
            Purchases = new()
        });
    }

    public async Task<CustomerDashboardDto> GetCustomerDashboardAsync(int customerId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == customerId && u.Role == "customer");
        if (user == null)
            throw new KeyNotFoundException("Customer not found");

        var vehicles = await _db.Vehicles.Where(v => v.UserId == customerId).ToListAsync();
        var appointments = await _db.Appointments.Where(a => a.CustomerId == customerId).ToListAsync();
        var purchases = await _db.SalesInvoices
            .Include(s => s.Items)
            .ThenInclude(i => i.Part)
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();
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
                    IsPaid = p.PaymentStatus?.ToLower() == "paid",
                    InvoiceNumber = $"INV-{p.SaleId}",
                    Items = p.Items.Select(i => new PurchaseItemDto
                    {
                        PartId = i.PartId,
                        PartName = i.Part.PartName,
                        Quantity = i.Quantity,
                        UnitPrice = (decimal)i.UnitPrice,
                        TotalPrice = (decimal)i.TotalPrice
                    }).ToList()
                }).ToList()
        };
    }

    public async Task<IEnumerable<Appointment>> GetServiceHistoryAsync(int customerId)
        => await _db.Appointments
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.ScheduledAt)
            .ToListAsync();
}
