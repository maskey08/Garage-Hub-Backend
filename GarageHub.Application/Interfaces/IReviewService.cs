using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Application.DTOs.Review;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Interfaces;

public interface IReviewService
{
    Task<Review> SubmitAsync(int customerId, ReviewCreateDto dto);
    Task<IEnumerable<Review>> GetByCustomerAsync(int customerId);
}