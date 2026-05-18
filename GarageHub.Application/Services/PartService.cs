using GarageHub.Application.DTOs.Part;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Services
{
    public class PartService : IPartService
    {
        private readonly IRepository<Part> _partRepository;

        public PartService(IRepository<Part> partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<IEnumerable<PartDto>> GetAllPartsAsync()
        {
            var parts = await _partRepository.GetAllAsync();
            return parts.Select(p => new PartDto
            {
                Id = p.Id,
                Name = p.Name,
                PartNumber = p.PartNumber,
                Description = p.Description,
                Price = p.Price,
                QuantityInStock = p.QuantityInStock,
                ReorderLevel = p.ReorderLevel,
                VendorId = p.VendorId,
                VendorName = p.Vendor?.Name
            });
        }

        public async Task<PartDto?> GetPartByIdAsync(int id)
        {
            var p = await _partRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new PartDto
            {
                Id = p.Id,
                Name = p.Name,
                PartNumber = p.PartNumber,
                Description = p.Description,
                Price = p.Price,
                QuantityInStock = p.QuantityInStock,
                ReorderLevel = p.ReorderLevel,
                VendorId = p.VendorId,
                VendorName = p.Vendor?.Name
            };
        }

        public async Task<PartDto> CreatePartAsync(CreatePartDto dto)
        {
            var part = new Part
            {
                Name = dto.Name,
                PartNumber = dto.PartNumber,
                Description = dto.Description,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock,
                ReorderLevel = dto.ReorderLevel,
                VendorId = dto.VendorId
            };
            await _partRepository.AddAsync(part);
            return new PartDto { Id = part.Id, Name = part.Name, PartNumber = part.PartNumber };
        }

        public async Task<PartDto?> UpdatePartAsync(int id, UpdatePartDto dto)
        {
            var part = await _partRepository.GetByIdAsync(id);
            if (part == null) return null;
            part.Name = dto.Name;
            part.Description = dto.Description;
            part.Price = dto.Price;
            part.QuantityInStock = dto.QuantityInStock;
            part.ReorderLevel = dto.ReorderLevel;
            part.VendorId = dto.VendorId;
            await _partRepository.UpdateAsync(part);
            return new PartDto { Id = part.Id, Name = part.Name };
        }

        public async Task<bool> DeletePartAsync(int id)
        {
            var part = await _partRepository.GetByIdAsync(id);
            if (part == null) return false;
            await _partRepository.DeleteAsync(part);
            return true;
        }
    }
}