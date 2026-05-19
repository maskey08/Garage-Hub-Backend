using GarageHub.Application.DTOs.Auth;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GarageHub.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserProfileService _userProfileService;
    private readonly AppDbContext _db;

    public AuthService(IConfiguration configuration, IUserProfileService userProfileService, AppDbContext db)
    {
        _configuration = configuration;
        _userProfileService = userProfileService;
        _db = db;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
        {
            return new AuthResponseDto { Success = false, Message = "Email already exists" };
        }

        var user = new User
        {
            Email = dto.Email,
            Phone = dto.Phone,
            FirstName = dto.FirstName ?? string.Empty,
            LastName = dto.LastName ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHashText = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var role = !string.IsNullOrEmpty(dto.Role) ? dto.Role : "customer";
        user.Role = role;
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // ✅ Sync to custom users table
        await _userProfileService.CreateUserProfileAsync(user, role);

        // ✅ If customer registration includes vehicle details, save them
        if (role == "customer" && !string.IsNullOrWhiteSpace(dto.VehicleNumber))
        {
            var vehicle = new Vehicle
            {
                UserId = user.UserId,
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
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
        {
            return new AuthResponseDto { Success = false, Message = "Invalid credentials" };
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHashText);
        if (!isPasswordValid)
        {
            return new AuthResponseDto { Success = false, Message = "Invalid credentials" };
        }

        var role = string.IsNullOrEmpty(user.Role) ? "customer" : user.Role;

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
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role),
            new Claim("role", role) // Add both standard and custom role claims
        };

        var expireMinutes = int.Parse(_configuration["Jwt:ExpireinMinutes"] ?? "60");
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
