using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.API.Controllers;

/// <summary>
/// Diagnostic endpoint to check database state and user data
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController : ControllerBase
{
    private readonly AppDbContext _db;

    public DiagnosticsController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all users in the database
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _db.Users
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Phone,
                    u.EmailConfirmed,
                    u.CreatedAt,
                    PasswordHashExists = !string.IsNullOrEmpty(u.PasswordHash)
                })
                .ToListAsync();

            return Ok(new
            {
                TotalUsers = users.Count,
                Users = users
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var roles = await _db.Roles.ToListAsync();
            return Ok(new
            {
                TotalRoles = roles.Count,
                Roles = roles.Select(r => new { r.Id, r.Name })
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get user roles mapping
    /// </summary>
    [HttpGet("user-roles")]
    public async Task<IActionResult> GetUserRoles()
    {
        try
        {
            var users = await _db.Users.ToListAsync();
            var userRolesList = new List<object>();

            foreach (var user in users)
            {
                var userRoles = await _db.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        RoleId = r.Id,
                        RoleName = r.Name
                    })
                    .ToListAsync();

                if (userRoles.Any())
                {
                    userRolesList.AddRange(userRoles);
                }
            }

            return Ok(new
            {
                Total = userRolesList.Count,
                UserRoles = userRolesList
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Check database connection and tables
    /// </summary>
    [HttpGet("db-status")]
    public async Task<IActionResult> CheckDatabaseStatus()
    {
        try
        {
            var canConnect = await _db.Database.CanConnectAsync();

            // Try to count users
            var userCount = 0;
            var roleCount = 0;
            var userRoleCount = 0;
            var userProfileCount = 0;

            try
            {
                userCount = await _db.Users.CountAsync();
                roleCount = await _db.Roles.CountAsync();
                userRoleCount = await _db.UserRoles.CountAsync();
                // UserProfiles is now deprecated - User entity maps directly to users table
                userProfileCount = userCount; // Same count as users
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Database connected but error querying tables",
                    details = ex.Message
                });
            }

            return Ok(new
            {
                DatabaseConnected = canConnect,
                Tables = new
                {
                    Users = userCount,
                    Roles = roleCount,
                    UserRoles = userRoleCount,
                    Message = "UserProfile entity is deprecated - User table is now consolidated"
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Database connection failed: {ex.Message}" });
        }
    }

    /// <summary>
    /// Get user profiles (now directly from Users table)
    /// </summary>
    [HttpGet("user-profiles")]
    public async Task<IActionResult> GetUserProfiles()
    {
        try
        {
            var profiles = await _db.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Phone,
                    u.UserName,
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(new
            {
                TotalProfiles = profiles.Count,
                UserProfiles = profiles
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Check system status (User table is now consolidated)
    /// </summary>
    [HttpGet("sync-status")]
    public async Task<IActionResult> CheckSyncStatus()
    {
        try
        {
            var totalUsers = await _db.Users.CountAsync();
            var staffUsers = await _db.Users
                .Where(u => _db.UserRoles
                    .Any(ur => ur.UserId == u.Id && 
                        _db.Roles.Any(r => r.Id == ur.RoleId && (r.Name == "staff" || r.Name == "admin"))))
                .CountAsync();

            var syncIssues = new List<object>();

            // Since User table is now consolidated, no sync issues expected
            if (totalUsers == 0)
            {
                syncIssues.Add(new { Issue = "No users in database" });
            }

            return Ok(new
            {
                SyncStatus = "✅ UNIFIED",
                TotalUsers = totalUsers,
                StaffUsers = staffUsers,
                CustomerUsers = totalUsers - staffUsers,
                SyncIssues = syncIssues,
                Message = "User table is now unified - no sync issues"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
