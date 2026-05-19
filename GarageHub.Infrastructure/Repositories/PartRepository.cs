using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Repositories
{
    public class PartRepository : IPartRepository
    {
        private readonly AppDbContext _context;

        public PartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Part?> GetPartByIdAsync(int partId)
        {
            return await _context.Parts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == partId);
        }

        public async Task UpdatePartAsync(Part part)
        {
            _context.Parts.Update(part);
            await _context.SaveChangesAsync();
        }
    }
}