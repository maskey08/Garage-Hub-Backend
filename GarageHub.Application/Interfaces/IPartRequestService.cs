using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Application.DTOs.PartRequest;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Interfaces;

public interface IPartRequestService
{
    Task<PartRequest> SubmitAsync(int customerId, PartRequestCreateDto dto);
    Task<IEnumerable<PartRequest>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<PartRequest>> GetAllAsync();
}
