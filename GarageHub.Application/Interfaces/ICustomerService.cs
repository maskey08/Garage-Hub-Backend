using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto?> GetCustomerWithDetailsAsync(int id);
        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm, string searchBy);
    }
}