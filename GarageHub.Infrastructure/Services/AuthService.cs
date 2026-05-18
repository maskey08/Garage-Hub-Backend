using GarageHub.Application.DTOs.Auth;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GarageHub.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUserProfileService _userProfileService;
    private readonly AppDbContext _db;

    public AuthService(UserManager<User> userManager, IConfiguration configuration, IUserProfileService userProfileService, AppDbContext db)
    {
        _configuration = configuration;
        _userManager = userManager;
        _userProfileService = userProfileService;
        _db = db;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return new AuthResponseDto { Success = false, Message = "Email already exists" };

        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            Phone = dto.Phone,
            FirstName = dto.FirstName ?? string.Empty,
            LastName = dto.LastName ?? string.Empty,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new AuthResponseDto { Success = false, Message = "Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)) };

        var role = !string.IsNullOrEmpty(dto.Role) ? dto.Role : "customer";
        await _userManager.AddToRoleAsync(user, role);

        // ✅ Sync to custom users table
        await _userProfileService.CreateUserProfileAsync(user, role);

        // ✅ If customer registration includes vehicle details, save them
        if (role == "customer" && !string.IsNullOrWhiteSpace(dto.VehicleNumber))
        {
            var vehicle = new Vehicle
            {
                UserId = user.Id,
                VehicleNumber = dto.VehicleNumber,
                Make = dto.VehicleMake ?? string.Empty,
                Model = dto.VehicleModel ?? string.Empty,
                Year = dto.VehicleYear ?? DateTime.Now.Year,
                Vin = dto.VehicleVin ?? string.Empty
            };
            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();
        }

        return new AuthResponseDto
        {
            Success = true,
            Message = "Registration successful",
            Token = GenerateJwtToken(user, role),
            Role = role,
            FullName = $"{user.FirstName} {user.LastName}".Trim()
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return new AuthResponseDto { Success = false, Message = "Invalid credentials" };

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
            return new AuthResponseDto { Success = false, Message = "Invalid credentials" };

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "customer";

        return new AuthResponseDto
        {
            Success = true,
            Message = "Login successful",
            Token = GenerateJwtToken(user, role),
            Role = role,
            FullName = $"{user.FirstName} {user.LastName}".Trim()
        };
    }

    private string GenerateJwtToken(User user, string role)
    {
        var jwtKey = _configuration["Jwt:Key"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!),
        //new Claim("role", role),          // ✅ plain "role" key
        new Claim(ClaimTypes.Role, role), // ✅ also add standard one
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
