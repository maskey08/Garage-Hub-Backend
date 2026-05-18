using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> GetAllStaffAsync();
    Task<IEnumerable<StaffDto>> SearchStaffAsync(string searchTerm);
    Task<StaffDto> AddStaffAsync(AddStaffDto dto);
    Task<StaffDto> UpdateStaffAsync(int id, UpdateStaffDto dto);
    Task DeleteStaffAsync(int id);
    Task<bool> CreateStaffAsync(int userId);
    Task<bool> RemoveStaffAsync(int userId);
}