using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services;

public class StaffService : IStaffService
{
    private readonly AppDbContext _db;
    private readonly UserManager<User> _userManager;

    public StaffService(AppDbContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
    {
        var staffUsers = await _db.Users
            .Where(u => _db.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                    _db.Roles.Any(r => r.Id == ur.RoleId && (r.Name == "staff" || r.Name == "admin"))))
            .ToListAsync();

        return staffUsers.Select(u => new StaffDto
        {
            Id = u.Id,
            Name = $"{u.FirstName} {u.LastName}".Trim(),
            Email = u.Email ?? "",
            Phone = u.Phone ?? "",
            Role = "staff",
            Status = "active"
        });
    }

    public async Task<StaffDto> AddStaffAsync(AddStaffDto dto)
    {
        var exists = await _userManager.FindByEmailAsync(dto.Email);
        if (exists != null)
            throw new Exception("Email already registered.");

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

        var result = await _userManager.CreateAsync(user,
            string.IsNullOrEmpty(dto.Password) ? "DefaultPass@123" : dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create staff: {errors}");
        }

        await _userManager.AddToRoleAsync(user, "staff");

        return new StaffDto
        {
            Id = user.Id,
            Name = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? "",
            Phone = user.Phone ?? "",
            Role = "staff",
            Status = "active"
        };
    }

    public async Task<StaffDto> UpdateStaffAsync(int id, UpdateStaffDto dto)
    {
        var user = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new Exception("Staff member not found.");

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.Phone = dto.Phone ?? user.Phone;

        await _userManager.UpdateAsync(user);

        return new StaffDto
        {
            Id = user.Id,
            Name = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? "",
            Phone = user.Phone ?? "",
            Role = "staff",
            Status = "active"
        };
    }

    public async Task DeleteStaffAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new Exception("Staff member not found.");
        await _userManager.DeleteAsync(user);
    }

    public async Task<bool> CreateStaffAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (!currentRoles.Contains("staff"))
        {
            await _userManager.AddToRoleAsync(user, "staff");
        }
        return true;
    }

    public async Task<bool> RemoveStaffAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains("staff"))
        {
            await _userManager.RemoveFromRoleAsync(user, "staff");
        }
        return true;
    }
}