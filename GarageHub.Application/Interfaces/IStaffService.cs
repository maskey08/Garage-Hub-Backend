using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces;

public interface IStaffService
{
    Task<List<StaffDto>> GetAllStaffAsync();
    Task<StaffDto> GetStaffByIdAsync(int id);
    Task<bool> CreateStaffAsync(int userId);
    Task<bool> RemoveStaffAsync(int userId);
}
