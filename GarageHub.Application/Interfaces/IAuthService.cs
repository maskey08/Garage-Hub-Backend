using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<string?> RegisterAsync(RegisterDto registerDto);
        Task<IEnumerable<StaffDto>> GetAllStaffAsync();
        Task<StaffDto?> GetStaffByIdAsync(int id);
        Task<string?> UpdateStaffAsync(int id, UpdateStaffDto updateStaffDto);
        Task<bool> DeleteStaffAsync(int id);
        Task<string?> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    }
}