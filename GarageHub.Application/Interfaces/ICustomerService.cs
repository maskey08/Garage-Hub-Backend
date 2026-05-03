using GarageHub.Application.DTOs;

namespace GarageHub.Application.Interfaces;

public interface ICustomerService
{
    // Feature 10: Staff search customers
    Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm, string searchBy);
    
    // Feature 8: Staff view customer details with history
    Task<CustomerDto?> GetCustomerWithDetailsAsync(int id);
}