using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Repositories;

namespace GarageHub.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUserRepository _userRepository;

    public CustomerService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Feature 10: Staff search customers (users with role = customer)
    public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm, string searchBy)
    {
        var allUsers = await _userRepository.GetAllAsync();
        var customers = allUsers.Where(u => u.Role.ToLower() == "customer");
        
        switch (searchBy.ToLower())
        {
            case "name":
                customers = customers.Where(c => c.FullName.Contains(searchTerm));
                break;
            case "phone":
                customers = customers.Where(c => c.Phone.Contains(searchTerm));
                break;
            case "id":
                if (int.TryParse(searchTerm, out int id))
                    customers = customers.Where(c => c.UserId == id);
                break;
            default:
                return new List<CustomerDto>();
        }
        
        return customers.Select(MapToDto);
    }

    // Feature 8: Staff view customer details with history
    public async Task<CustomerDto?> GetCustomerWithDetailsAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null || user.Role.ToLower() != "customer")
            return null;
        
        return MapToDto(user);
    }

    private static CustomerDto MapToDto(User user)
    {
        return new CustomerDto
        {
            Id = user.UserId,
            FullName = user.FullName,
            Phone = user.Phone,
            Email = user.Email,
            RegisteredDate = user.CreatedAt,
            CreditBalance = 0,
            Vehicles = new List<VehicleDto>(),
            Purchases = new List<PurchaseSummaryDto>()
        };
    }
}