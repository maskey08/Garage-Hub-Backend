using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services;

/// <summary>
/// Service to manage user profile data (now directly in User entity)
/// </summary>
public interface IUserProfileService
{
    Task CreateUserProfileAsync(User identityUser, string role = "customer");
    Task UpdateUserProfileAsync(User identityUser, string role);
    Task DeleteUserProfileAsync(int userId);
    Task<User?> GetUserProfileAsync(int userId);
}

public class UserProfileService : IUserProfileService
{
    private readonly AppDbContext _db;

    public UserProfileService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Ensure user profile data is set up (now works directly with User entity)
    /// </summary>
    public async Task CreateUserProfileAsync(User identityUser, string role = "customer")
    {
        // User entity already has all profile data via User properties
        // Just update the user if needed
        var user = await _db.Users.FindAsync(identityUser.Id);

        if (user != null)
        {
            // User already exists, ensure profile fields are populated
            user.FirstName = identityUser.FirstName;
            user.LastName = identityUser.LastName;
            user.Phone = identityUser.Phone ?? string.Empty;
            user.CreatedAt = identityUser.CreatedAt;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Update user profile (now works directly with User entity)
    /// </summary>
    public async Task UpdateUserProfileAsync(User identityUser, string role)
    {
        var user = await _db.Users.FindAsync(identityUser.Id);

        if (user == null)
        {
            // If user doesn't exist, create profile
            await CreateUserProfileAsync(identityUser, role);
            return;
        }

        user.FirstName = identityUser.FirstName;
        user.LastName = identityUser.LastName;
        user.Email = identityUser.Email ?? string.Empty;
        user.Phone = identityUser.Phone ?? string.Empty;

        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Delete user profile (cascade delete via User entity)
    /// </summary>
    public async Task DeleteUserProfileAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);

        if (user != null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Get user profile (now returns User entity directly)
    /// </summary>
    public async Task<User?> GetUserProfileAsync(int userId)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}
