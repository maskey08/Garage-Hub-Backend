using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var error = await _authService.RegisterAsync(registerDto);
            if (error != null)
                return BadRequest(new { message = error });

            return Ok(new { message = "User registered successfully" });
        }

        [HttpGet("staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staff = await _authService.GetAllStaffAsync();
            return Ok(staff);
        }

        [HttpGet("staff/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStaffById(int id)
        {
            var staff = await _authService.GetStaffByIdAsync(id);
            if (staff == null)
                return NotFound(new { message = "Staff not found" });

            return Ok(staff);
        }

        [HttpPut("staff/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] UpdateStaffDto updateStaffDto)
        {
            var error = await _authService.UpdateStaffAsync(id, updateStaffDto);
            if (error != null)
                return BadRequest(new { message = error });

            return Ok(new { message = "Staff updated successfully" });
        }

        [HttpDelete("staff/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var result = await _authService.DeleteStaffAsync(id);
            if (!result)
                return NotFound(new { message = "Staff not found" });

            return Ok(new { message = "Staff deactivated successfully" });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var error = await _authService.ChangePasswordAsync(userId, changePasswordDto);
            if (error != null)
                return BadRequest(new { message = error });

            return Ok(new { message = "Password changed successfully" });
        }
    }
}