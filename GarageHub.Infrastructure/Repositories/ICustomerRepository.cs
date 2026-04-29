using GarageHub.Domain.Entities;

namespace GarageHub.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetCustomerWithDetailsAsync(int id);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, string searchBy);
        Task<Customer?> GetCustomerByUserIdAsync(int userId);
    }
}