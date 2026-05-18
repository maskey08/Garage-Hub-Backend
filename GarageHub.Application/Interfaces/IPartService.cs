using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces;

public interface IPartService
{
    Task<List<PartDto>> GetPartsAsync(string? search, string? category);
    Task<List<LowStockPartDto>> GetLowStockPartsAsync();
    Task<PartDto> AddPartAsync(CreatePartDto dto);
    Task<PartDto> UpdatePartAsync(int id, UpdatePartDto dto);
    Task<bool> DeletePartAsync(int id);
}
