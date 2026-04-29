using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Domain.Enums;
using GarageHub.Infrastructure.Repositories;

namespace GarageHub.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                return null;

            if (!user.IsActive)
                return null;

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        public async Task<string?> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                return "Email already exists";

            // Parse role
            if (!Enum.TryParse<UserRole>(registerDto.Role, true, out var role))
                role = UserRole.Staff;

            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                PasswordHash = HashPassword(registerDto.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.CreateAsync(user);
            return null; // Success
        }

        public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
        {
            var staff = await _userRepository.GetAllStaffAsync();
            return staff.Select(MapToStaffDto);
        }

        public async Task<StaffDto?> GetStaffByIdAsync(int id)
        {
            var staff = await _userRepository.GetByIdAsync(id);
            return staff != null ? MapToStaffDto(staff) : null;
        }

        public async Task<string?> UpdateStaffAsync(int id, UpdateStaffDto updateStaffDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return "Staff not found";

            user.FullName = updateStaffDto.FullName;
            user.Phone = updateStaffDto.Phone;
            user.IsActive = updateStaffDto.IsActive;

            if (Enum.TryParse<UserRole>(updateStaffDto.Role, true, out var role))
                user.Role = role;

            await _userRepository.UpdateAsync(user);
            return null;
        }

        public async Task<bool> DeleteStaffAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<string?> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return "User not found";

            if (!VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
                return "Current password is incorrect";

            user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            await _userRepository.UpdateAsync(user);
            return null;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "GarageHubSecretKey12345678901234567890"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private StaffDto MapToStaffDto(User user)
        {
            return new StaffDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }
    }
}