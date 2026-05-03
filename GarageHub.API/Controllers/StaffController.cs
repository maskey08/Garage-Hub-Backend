using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;
    private readonly UserManager<User> _userManager;

    public StaffController(IStaffService staffService, UserManager<User> userManager)
    {
        _staffService = staffService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStaff()
    {
        try
        {
            var staff = await _staffService.GetAllStaffAsync();
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffById(int id)
    {
        try
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    // Add staff endpoint
    [HttpPost]
    public async Task<IActionResult> AddStaff([FromBody] AddStaffDto dto)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email already exists" });

            // Create new staff user
            var newUser = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName ?? string.Empty,
                LastName = dto.LastName ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            // Create user with password
            var result = await _userManager.CreateAsync(newUser, dto.Password ?? "DefaultPass@123");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"User creation failed: {errors}" });
            }

            // Assign staff role
            await _userManager.AddToRoleAsync(newUser, "staff");

            // Return created staff
            var staffDto = new
            {
                id = newUser.Id,
                name = $"{newUser.FirstName} {newUser.LastName}".Trim(),
                email = newUser.Email,
                phone = newUser.Phone,
                role = "staff",
                status = "active",
                createdAt = newUser.CreatedAt
            };

            return Ok(staffDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Update staff
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(int id, [FromBody] UpdateStaffDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { message = "Staff member not found" });

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Phone = dto.Phone ?? user.Phone;
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                user.Email = dto.Email;
                user.UserName = dto.Email;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Update failed: {errors}" });
            }

            return Ok(new { message = "Staff member updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Delete staff
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { message = "Staff member not found" });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Delete failed: {errors}" });
            }

            return Ok(new { message = "Staff member deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{userId}/assign")]
    public async Task<IActionResult> AssignStaffRole(int userId)
    {
        try
        {
            var result = await _staffService.CreateStaffAsync(userId);
            return result ? Ok("Staff role assigned successfully") : BadRequest("Failed to assign staff role");
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpDelete("{userId}/remove")]
    public async Task<IActionResult> RemoveStaffRole(int userId)
    {
        try
        {
            var result = await _staffService.RemoveStaffAsync(userId);
            return result ? Ok("Staff role removed successfully") : BadRequest("Failed to remove staff role");
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}

