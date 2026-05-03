using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services;

public class StaffService : IStaffService
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<Domain.Entities.User> _userManager;

    public StaffService(AppDbContext dbContext, UserManager<Domain.Entities.User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<List<StaffDto>> GetAllStaffAsync()
    {
        var staffRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "staff");
        if (staffRole == null)
            return [];

        var staffUserIds = await _dbContext.UserRoles
            .Where(ur => ur.RoleId == staffRole.Id)
            .Select(ur => ur.UserId)
            .ToListAsync();

        var staff = await _dbContext.Users
            .Where(u => staffUserIds.Contains(u.Id))
            .Select(u => new StaffDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                FullName = u.UserName ?? string.Empty,
                Phone = u.Phone,
                Role = "staff",
                CreatedAt = u.CreatedAt,
                IsActive = !u.LockoutEnabled
            })
            .ToListAsync();

        return staff;
    }

    public async Task<StaffDto> GetStaffByIdAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new Exception("Staff member not found");

        var isStaff = await _userManager.IsInRoleAsync(user, "staff");
        if (!isStaff)
            throw new Exception("User is not a staff member");

        return new StaffDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.UserName ?? string.Empty,
            Phone = user.Phone,
            Role = "staff",
            CreatedAt = user.CreatedAt,
            IsActive = !user.LockoutEnabled
        };
    }

    public async Task<bool> CreateStaffAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new Exception("User not found");

        var result = await _userManager.AddToRoleAsync(user, "staff");
        return result.Succeeded;
    }

    public async Task<bool> RemoveStaffAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new Exception("User not found");

        var result = await _userManager.RemoveFromRoleAsync(user, "staff");
        return result.Succeeded;
    }
}
