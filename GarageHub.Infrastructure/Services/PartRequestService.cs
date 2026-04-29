using GarageHub.Application.DTOs.PartRequest;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services;

public class PartRequestService : IPartRequestService
{
    private readonly AppDbContext _db;
    public PartRequestService(AppDbContext db) => _db = db;

    public async Task<PartRequest> SubmitAsync(int customerId, PartRequestCreateDto dto)
    {
        var request = new PartRequest
        {
            CustomerId = customerId,
            PartName = dto.PartName,
            Description = dto.Description
        };
        _db.PartRequests.Add(request);
        await _db.SaveChangesAsync();
        return request;
    }

    public async Task<IEnumerable<PartRequest>> GetByCustomerAsync(int customerId)
        => await _db.PartRequests
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.RequestedAt)
            .ToListAsync();
}