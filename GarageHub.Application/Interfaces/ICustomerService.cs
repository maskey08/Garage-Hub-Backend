using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Interfaces;

public interface ICustomerService
{
    Task<User?> GetProfileAsync(int userId);
    Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string phone);
    Task<IEnumerable<SalesInvoice>> GetPurchaseHistoryAsync(int customerId);
    Task<IEnumerable<Vehicle>> GetVehiclesAsync(int customerId);
    Task<Vehicle> AddVehicleAsync(int customerId, Vehicle vehicle);
}