using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Repositories;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(MapToDto);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            return customer != null ? MapToDto(customer) : null;
        }

        public async Task<CustomerDto?> GetCustomerWithDetailsAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerWithDetailsAsync(id);
            return customer != null ? MapToDetailDto(customer) : null;
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm, string searchBy)
        {
            var customers = await _customerRepository.SearchCustomersAsync(searchTerm, searchBy);
            return customers.Select(MapToDto);
        }

        private static CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                RegisteredDate = customer.RegisteredDate,
                CreditBalance = customer.CreditBalance
            };
        }

        private static CustomerDto MapToDetailDto(Customer customer)
        {
            var dto = MapToDto(customer);

            dto.Vehicles = customer.Vehicles?.Select(v => new VehicleDto
            {
                Id = v.Id,
                VehicleNumber = v.VehicleNumber,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                Color = v.Color
            }).ToList() ?? new();

            dto.Purchases = customer.Purchases?.Select(p => new PurchaseSummaryDto
            {
                Id = p.Id,
                PurchaseDate = p.PurchaseDate,
                TotalAmount = p.TotalAmount,
                IsPaid = p.IsPaid
            }).ToList() ?? new();

            return dto;
        }
    }
}