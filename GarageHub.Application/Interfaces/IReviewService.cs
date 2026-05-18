using GarageHub.Application.DTOs.Review;

namespace GarageHub.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewResponseDto> SubmitAsync(int customerId, ReviewCreateDto dto);
    Task<IEnumerable<ReviewResponseDto>> GetByCustomerAsync(int customerId);
}