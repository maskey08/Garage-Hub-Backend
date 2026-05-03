using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services;

public class PartService : IPartService
{
    private readonly AppDbContext _dbContext;

    public PartService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PartDto>> GetPartsAsync(string? search, string? category)
    {
        var query = _dbContext.Parts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.Trim().ToLower();
            query = query.Where(p => p.PartName.ToLower().Contains(lowered)
                                     || p.PartNumber.ToLower().Contains(lowered)
                                     || p.Brand.ToLower().Contains(lowered));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category == category);
        }

        var parts = await query.OrderBy(p => p.PartName).ToListAsync();

        return parts.Select(MapToDto).ToList();
    }

    public async Task<List<LowStockPartDto>> GetLowStockPartsAsync()
    {
        return await _dbContext.Parts
            .Where(p => p.StockQuantity <= p.LowStockThreshold)
            .OrderBy(p => p.StockQuantity)
            .Select(p => new LowStockPartDto
            {
                PartId = p.Id,
                PartName = p.PartName,
                Sku = p.PartNumber,
                CurrentStock = p.StockQuantity,
                MinThreshold = p.LowStockThreshold
            })
            .ToListAsync();
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
            StockQuantity = dto.Quantity,
            LowStockThreshold = dto.LowStockThreshold
        };

        _dbContext.Parts.Add(part);
        await _dbContext.SaveChangesAsync();

        return MapToDto(part);
    }

    public async Task<PartDto> UpdatePartAsync(int id, UpdatePartDto dto)
    {
        var part = await _dbContext.Parts.FirstOrDefaultAsync(p => p.Id == id);
        if (part == null)
            throw new Exception("Part not found");

        part.PartName = dto.Name ?? part.PartName;
        part.PartNumber = dto.Sku ?? part.PartNumber;
        part.Brand = dto.Brand ?? part.Brand;
        part.Category = dto.Category ?? part.Category;
        part.Price = dto.Price ?? part.Price;
        part.StockQuantity = dto.Quantity ?? part.StockQuantity;
        part.LowStockThreshold = dto.LowStockThreshold ?? part.LowStockThreshold;

        await _dbContext.SaveChangesAsync();

        return MapToDto(part);
    }

    public async Task<bool> DeletePartAsync(int id)
    {
        var part = await _dbContext.Parts.FirstOrDefaultAsync(p => p.Id == id);
        if (part == null)
            return false;

        _dbContext.Parts.Remove(part);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private static PartDto MapToDto(Part part)
    {
        var status = part.StockQuantity <= 0
            ? "out_of_stock"
            : part.StockQuantity <= part.LowStockThreshold
                ? "low_stock"
                : "in_stock";

        return new PartDto
        {
            Id = part.Id,
            Name = part.PartName,
            Sku = part.PartNumber,
            Brand = part.Brand,
            Category = part.Category,
            Price = part.Price,
            Quantity = part.StockQuantity,
            Status = status,
            LowStockThreshold = part.LowStockThreshold
        };
    }
}
