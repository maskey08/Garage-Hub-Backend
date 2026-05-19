using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Services;

public class StaffService : IStaffService
{
    private readonly AppDbContext _db;
    private readonly IUserProfileService _userProfileService;

    public StaffService(AppDbContext db, IUserProfileService userProfileService)
    {
        _db = db;
        _userProfileService = userProfileService;
    }

    public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
    {
        var staffUsers = await _db.Users
            .Where(u => u.Role == "staff" || u.Role == "admin")
            .ToListAsync();

        return staffUsers.Select(u => new StaffDto
        {
            Id = u.UserId,
            Name = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? "",
            Phone = u.Phone ?? "",
            Role = u.SubRole ?? u.Role,
            SubRole = u.SubRole,
            Status = "active"
        });
    }

    public async Task<StaffDto> AddStaffAsync(AddStaffDto dto)
    {
        var exists = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (exists != null)
        {
            throw new Exception("Email already registered.");
        }

        var subRole = NormalizeSubRole(dto.SubRole);

        var user = new User
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            SubRole = subRole,
            CreatedAt = DateTime.UtcNow
        };

        var password = string.IsNullOrEmpty(dto.Password) ? "DefaultPass@123" : dto.Password;
        user.PasswordHashText = BCrypt.Net.BCrypt.HashPassword(password);
        user.Role = "staff";
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Sync user data to the custom 'users' table
        await _userProfileService.CreateUserProfileAsync(user, "staff");

        return new StaffDto
        {
            Id = user.UserId,
            Name = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? "",
            Phone = user.Phone ?? "",
            Role = user.SubRole ?? user.Role,
            SubRole = user.SubRole,
            Status = "active"
        };
    }

    public async Task<StaffDto> UpdateStaffAsync(int id, UpdateStaffDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id)
            ?? throw new Exception("Staff member not found.");

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.Phone = dto.Phone ?? user.Phone;
        user.Role = string.IsNullOrEmpty(user.Role) ? "staff" : user.Role;
        user.SubRole = NormalizeSubRole(dto.SubRole) ?? user.SubRole;

        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return new StaffDto
        {
            Id = user.UserId,
            Name = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? "",
            Phone = user.Phone ?? "",
            Role = user.SubRole ?? user.Role,
            SubRole = user.SubRole,
            Status = "active"
        };
    }

    public async Task DeleteStaffAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id)
            ?? throw new Exception("Staff member not found.");
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> CreateStaffAsync(int userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            return false;
        }

        user.Role = "staff";
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveStaffAsync(int userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            return false;
        }

        user.Role = "customer";
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StaffDto>> SearchStaffAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllStaffAsync();

        var searchLower = searchTerm.ToLower();
        var staffUsers = await _db.Users
            .Where(u => (u.Role == "staff" || u.Role == "admin")
                && (u.FirstName.ToLower().Contains(searchLower) ||
                    u.LastName.ToLower().Contains(searchLower) ||
                    u.Email!.ToLower().Contains(searchLower) ||
                    u.Phone.ToLower().Contains(searchLower)))
            .ToListAsync();

        return staffUsers.Select(u => new StaffDto
        {
            Id = u.UserId,
            Name = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? "",
            Phone = u.Phone ?? "",
            Role = u.SubRole ?? u.Role,
            SubRole = u.SubRole,
            Status = "active"
        });
    }

    private static string? NormalizeSubRole(string? subRole)
    {
        if (string.IsNullOrWhiteSpace(subRole))
        {
            return null;
        }

        var normalized = subRole.Trim();
        return normalized switch
        {
            "inventory_manager" => "inventory_manager",
            "Inventory manager" => "inventory_manager",
            "administrator" => "administrator",
            "Administrator" => "administrator",
            "sales associate" => "sales_associate",
            "sales_associate" => "sales_associate",
            "Sales associate" => "sales_associate",
            "vendor_liaison" => "vendor_liaison",
            "vendor Liason" => "vendor_liaison",
            "Vendor Liason" => "vendor_liaison",
            _ => throw new Exception("SubRole must be Administrator, Inventory manager, sales associate, or vendor Liason.")
        };
    }
}
