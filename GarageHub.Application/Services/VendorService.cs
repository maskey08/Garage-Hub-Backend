using GarageHub.Application.DTOs.Vendor;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Services
{
    public class VendorService : IVendorService
    {
        private readonly IRepository<Vendor> _vendorRepository;

        public VendorService(IRepository<Vendor> vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync()
        {
            var vendors = await _vendorRepository.GetAllAsync();
            return vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                ContactPerson = v.ContactPerson,
                Phone = v.Phone,
                Email = v.Email,
                Address = v.Address,
                CreatedAt = v.CreatedAt
            });
        }

        public async Task<VendorDto?> GetVendorByIdAsync(int id)
        {
            var v = await _vendorRepository.GetByIdAsync(id);
            if (v == null) return null;
            return new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                ContactPerson = v.ContactPerson,
                Phone = v.Phone,
                Email = v.Email,
                Address = v.Address,
                CreatedAt = v.CreatedAt
            };
        }

        public async Task<VendorDto> CreateVendorAsync(CreateVendorDto dto)
        {
            var vendor = new Vendor
            {
                Name = dto.Name,
                ContactPerson = dto.ContactPerson,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };
            await _vendorRepository.AddAsync(vendor);
            return new VendorDto { Id = vendor.Id, Name = vendor.Name };
        }

        public async Task<VendorDto?> UpdateVendorAsync(int id, UpdateVendorDto dto)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null) return null;
            vendor.Name = dto.Name;
            vendor.ContactPerson = dto.ContactPerson;
            vendor.Phone = dto.Phone;
            vendor.Email = dto.Email;
            vendor.Address = dto.Address;
            await _vendorRepository.UpdateAsync(vendor);
            return new VendorDto { Id = vendor.Id, Name = vendor.Name };
        }

        public async Task<bool> DeleteVendorAsync(int id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null) return false;
            await _vendorRepository.DeleteAsync(vendor);
            return true;
        }
    }
}