using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/staff/customers")]
[Authorize(Roles = "staff,admin")]
public class StaffCustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly AppDbContext _db;

    public StaffCustomersController(ICustomerService customerService, AppDbContext db)
    {
        _customerService = customerService;
        _db = db;
    }

    /// <summary>
    /// Get customer detail including vehicles
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerDetails(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerDetailsAsync(id);
            return Ok(customer);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get customer purchase history with item details
    /// </summary>
    [HttpGet("{id:int}/purchases")]
    public async Task<IActionResult> GetCustomerPurchases(int id)
    {
        try
        {
            var purchases = await _customerService.GetCustomerPurchaseHistoryAsync(id);
            return Ok(purchases);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all customers (staff can view all customers)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Search customers by name, email, or phone
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomers([FromQuery] string? searchTerm, [FromQuery] string? name, [FromQuery] string? phone, [FromQuery] string? vehicleNo, [FromQuery] string? id)
    {
        try
        {
            var resolvedSearch = searchTerm;
            if (string.IsNullOrWhiteSpace(resolvedSearch))
            {
                resolvedSearch = name;
            }
            if (string.IsNullOrWhiteSpace(resolvedSearch))
            {
                resolvedSearch = phone;
            }
            if (string.IsNullOrWhiteSpace(resolvedSearch))
            {
                resolvedSearch = vehicleNo;
            }
            if (string.IsNullOrWhiteSpace(resolvedSearch))
            {
                resolvedSearch = id;
            }

            var customers = await _customerService.SearchCustomersAsync(resolvedSearch ?? string.Empty);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new customer (staff member registering a new customer)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) || 
                string.IsNullOrWhiteSpace(dto.Email) || 
                string.IsNullOrWhiteSpace(dto.Phone) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "First Name, Email, Phone, and Password are required" });
            }

            if (dto.Password.Length < 8)
            {
                return BadRequest(new { message = "Password must be at least 8 characters long" });
            }

            var result = await _customerService.CreateCustomerAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}/appointments")]
    public async Task<IActionResult> GetCustomerAppointments(int id)
    {
        var appointments = await _db.Appointments
            .Include(a => a.Vehicle)
            .Where(a => a.CustomerId == id)
            .OrderByDescending(a => a.ScheduledAt)
            .Select(a => new
            {
                appointmentId = a.AppointmentId,
                scheduledAt = a.ScheduledAt,
                serviceType = a.ServiceType,
                status = a.Status,
                notes = a.Notes,
                vehicleNumber = a.Vehicle != null ? a.Vehicle.VehicleNumber : null,
                vehicleName = a.Vehicle != null ? (a.Vehicle.Make + " " + a.Vehicle.Model).Trim() : null
            })
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerRequestDto dto)
    {
        try
        {
            var customer = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id && u.Role == "customer");
            if (customer == null)
                return NotFound(new { message = "Customer not found" });

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName ?? string.Empty;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            await _db.SaveChangesAsync();

            return Ok(await _customerService.GetCustomerDetailsAsync(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id && u.Role == "customer");
        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        var appointments = await _db.Appointments.Where(a => a.CustomerId == id).ToListAsync();
        var appointmentIds = appointments.Select(a => a.AppointmentId).ToList();
        var reviews = await _db.Reviews
            .Where(r => r.CustomerId == id || appointmentIds.Contains(r.AppointmentId))
            .ToListAsync();
        var salesInvoices = await _db.SalesInvoices
            .Include(invoice => invoice.Items)
            .Where(invoice => invoice.CustomerId == id)
            .ToListAsync();
        var invoiceItems = salesInvoices.SelectMany(invoice => invoice.Items).ToList();
        var vehicles = await _db.Vehicles.Where(v => v.UserId == id).ToListAsync();
        var partRequests = await _db.PartRequests.Where(request => request.CustomerId == id).ToListAsync();
        var notifications = await _db.Notifications.Where(notification => notification.UserId == id).ToListAsync();

        _db.Reviews.RemoveRange(reviews);
        _db.Appointments.RemoveRange(appointments);
        _db.SalesInvoiceItems.RemoveRange(invoiceItems);
        _db.SalesInvoices.RemoveRange(salesInvoices);
        _db.Vehicles.RemoveRange(vehicles);
        _db.PartRequests.RemoveRange(partRequests);
        _db.Notifications.RemoveRange(notifications);
        _db.Users.Remove(customer);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:int}/vehicles")]
    public async Task<IActionResult> AddVehicle(int id, [FromBody] AddVehicleDto dto)
    {
        var customer = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id && u.Role == "customer");
        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        var vehicle = new GarageHub.Domain.Entities.Vehicle
        {
            UserId = id,
            VehicleNumber = dto.VehicleNumber,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            Vin = dto.Vin ?? string.Empty
        };

        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();

        return Ok(await _customerService.GetCustomerDetailsAsync(id));
    }

    [HttpPut("{id:int}/vehicles/{vehicleId:int}")]
    public async Task<IActionResult> UpdateVehicle(int id, int vehicleId, [FromBody] AddVehicleDto dto)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId && v.UserId == id);
        if (vehicle == null)
            return NotFound(new { message = "Vehicle not found" });

        vehicle.VehicleNumber = dto.VehicleNumber;
        vehicle.Make = dto.Make;
        vehicle.Model = dto.Model;
        vehicle.Year = dto.Year;
        vehicle.Vin = dto.Vin ?? string.Empty;
        await _db.SaveChangesAsync();

        return Ok(await _customerService.GetCustomerDetailsAsync(id));
    }

    [HttpDelete("{id:int}/vehicles/{vehicleId:int}")]
    public async Task<IActionResult> DeleteVehicle(int id, int vehicleId)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId && v.UserId == id);
        if (vehicle == null)
            return NotFound(new { message = "Vehicle not found" });

        _db.Vehicles.Remove(vehicle);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public class UpdateCustomerRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
}
