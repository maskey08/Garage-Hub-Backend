using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardStatisticsDto> GetStatisticsAsync();
    Task<List<TransactionDto>> GetRecentActivityAsync(int limit = 6);
}
