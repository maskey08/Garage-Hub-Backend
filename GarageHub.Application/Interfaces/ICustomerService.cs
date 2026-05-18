using GarageHub.Application.DTOs;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Interfaces;

public interface ICustomerService
{
    Task<User?> GetProfileAsync(int userId);
    Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone);
    Task<IEnumerable<SalesInvoice>> GetPurchaseHistoryAsync(int customerId);
    Task<IEnumerable<VehicleResponseDto>> GetVehiclesAsync(int customerId);
    Task<VehicleResponseDto> AddVehicleAsync(int customerId, AddVehicleDto vehicle);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm);
    Task<CustomerDashboardDto> GetCustomerDashboardAsync(int customerId);
    Task<IEnumerable<Appointment>> GetServiceHistoryAsync(int customerId);
}
