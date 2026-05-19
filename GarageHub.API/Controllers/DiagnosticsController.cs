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
                    u.UserId,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Phone,
                    u.Role,
                    u.CreatedAt,
                    PasswordHashExists = !string.IsNullOrEmpty(u.PasswordHashText)
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
            var roles = await _db.Users
                .Select(u => u.Role)
                .Where(r => !string.IsNullOrEmpty(r))
                .Distinct()
                .ToListAsync();
            return Ok(new
            {
                TotalRoles = roles.Count,
                Roles = roles.Select(r => new { Name = r })
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
            var userRolesList = await _db.Users
                .Select(u => new
                {
                    UserId = u.UserId,
                    UserEmail = u.Email,
                    RoleName = u.Role
                })
                .Where(u => !string.IsNullOrEmpty(u.RoleName))
                .ToListAsync();

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

            try
            {
                userCount = await _db.Users.CountAsync();
                roleCount = await _db.Users
                    .Select(u => u.Role)
                    .Where(r => !string.IsNullOrEmpty(r))
                    .Distinct()
                    .CountAsync();
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
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Phone,
                    u.Role,
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
                .CountAsync(u => u.Role == "staff" || u.Role == "admin");

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
