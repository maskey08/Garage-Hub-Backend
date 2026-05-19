using GarageHub.Application.DTOs.Vendor;

namespace GarageHub.Application.Interfaces
{
    public interface IVendorService
    {
        Task<IEnumerable<VendorDto>> GetAllVendorsAsync();
        Task<VendorDto?> GetVendorByIdAsync(int id);
        Task<VendorDto> CreateVendorAsync(CreateVendorDto dto);
        Task<VendorDto?> UpdateVendorAsync(int id, UpdateVendorDto dto);
        Task<bool> DeleteVendorAsync(int id);
    }
}
