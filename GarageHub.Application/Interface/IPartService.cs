using GarageHub.Application.DTOs.Part;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Interfaces
{
    public interface IPartService
    {
        Task<IEnumerable<PartDto>> GetAllPartsAsync();
        Task<PartDto?> GetPartByIdAsync(int id);
        Task<PartDto> CreatePartAsync(CreatePartDto dto);
        Task<PartDto?> UpdatePartAsync(int id, UpdatePartDto dto);
        Task<bool> DeletePartAsync(int id);
    }
}