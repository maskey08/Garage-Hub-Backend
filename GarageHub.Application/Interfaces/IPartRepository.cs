using GarageHub.Domain.Entities;
using System.Threading.Tasks;

namespace GarageHub.Application.Interfaces
{
    public interface IPartRepository
    {
        Task<Part?> GetPartByIdAsync(int partId);
        Task UpdatePartAsync(Part part);
    }
}