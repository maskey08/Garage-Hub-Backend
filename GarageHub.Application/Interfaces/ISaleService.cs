using System.Threading.Tasks;
using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces
{
    public interface ISaleService
    {
        Task<SaleResponseDto> CreateSaleAsync(CreateSaleRequestDto request);
    }
}