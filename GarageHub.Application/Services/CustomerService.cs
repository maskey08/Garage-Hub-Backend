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
        var customers = allUsers.Where(u => u.role.ToLower() == "customer");
        
        switch (searchBy.ToLower())
        {
            case "name":
                customers = customers.Where(c => (c.first_name + " " + c.last_name).ToLower().Contains(searchTerm.ToLower()));
                break;
            case "phone":
                customers = customers.Where(c => c.phone.Contains(searchTerm));
                break;
            case "id":
                if (int.TryParse(searchTerm, out int id))
                    customers = customers.Where(c => c.user_id == id);
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
        if (user == null || user.role.ToLower() != "customer")
            return null;
        
        return MapToDto(user);
    }

    private static CustomerDto MapToDto(User user)
    {
        return new CustomerDto
        {
            Id = user.user_id,
            FullName = $"{user.first_name} {user.last_name}".Trim(),
            Phone = user.phone,
            Email = user.email,
            RegisteredDate = user.created_at,
            CreditBalance = user.credit_balance,
            Address = "",
            Vehicles = new List<VehicleDto>(),
            Purchases = new List<PurchaseSummaryDto>()
        };
    }
}