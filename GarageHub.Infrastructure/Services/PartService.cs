using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;

namespace GarageHub.Infrastructure.Services
{
    public class PartService : IPartService
    {
        private readonly IRepository<Part> _partRepository;

        public PartService(IRepository<Part> partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<List<PartDto>> GetPartsAsync(string? search, string? category)
        {
            var parts = await _partRepository.GetAllAsync();
            var query = parts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowered = search.Trim().ToLowerInvariant();
                query = query.Where(p => p.PartName.ToLowerInvariant().Contains(lowered)
                                         || p.PartNumber.ToLowerInvariant().Contains(lowered)
                                         || p.Brand.ToLowerInvariant().Contains(lowered));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return query.OrderBy(p => p.PartName).Select(MapToDto).ToList();
        }

        public async Task<List<LowStockPartDto>> GetLowStockPartsAsync()
        {
            var parts = await _partRepository.GetAllAsync();
            return parts.Where(p => p.StockQuantity <= 10)
                .OrderBy(p => p.StockQuantity)
                .Select(p => new LowStockPartDto
                {
                    PartId = p.Id,
                    PartName = p.PartName,
                    Sku = string.IsNullOrEmpty(p.PartNumber) ? $"SKU-{p.Id}" : p.PartNumber,
                    CurrentStock = p.StockQuantity
                })
                .ToList();
        }

        public async Task<PartDto> AddPartAsync(CreatePartDto dto)
        {
            var part = new Part
            {
                PartName = dto.Name,
                PartNumber = dto.Sku,
                Brand = dto.Brand,
                Category = dto.Category,
                Price = dto.Price,
                StockQuantity = dto.Quantity
            };

            await _partRepository.AddAsync(part);
            return MapToDto(part);
        }

        public async Task<PartDto> UpdatePartAsync(int id, UpdatePartDto dto)
        {
            var part = await _partRepository.GetByIdAsync(id);
            if (part == null)
            {
                throw new InvalidOperationException("Part not found");
            }

            part.PartName = dto.Name ?? part.PartName;
            part.PartNumber = dto.Sku ?? part.PartNumber;
            part.Brand = dto.Brand ?? part.Brand;
            part.Category = dto.Category ?? part.Category;
            part.Price = dto.Price ?? part.Price;
            part.StockQuantity = dto.Quantity ?? part.StockQuantity;

            await _partRepository.UpdateAsync(part);
            return MapToDto(part);
        }

        public async Task<bool> DeletePartAsync(int id)
        {
            var part = await _partRepository.GetByIdAsync(id);
            if (part == null)
            {
                return false;
            }

            await _partRepository.DeleteAsync(part);
            return true;
        }

        private static PartDto MapToDto(Part part)
        {
            var status = part.StockQuantity <= 0
                ? "out_of_stock"
                : part.StockQuantity <= 10
                    ? "low_stock"
                    : "in_stock";

            return new PartDto
            {
                Id = part.Id,
                Name = part.PartName,
                Sku = string.IsNullOrEmpty(part.PartNumber) ? $"SKU-{part.Id}" : part.PartNumber,
                Brand = part.Brand,
                Category = part.Category,
                Price = part.Price,
                Quantity = part.StockQuantity,
                Status = status,
                LowStockThreshold = 10
            };
        }
    }
}
