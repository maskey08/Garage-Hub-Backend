using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/staff/customers")]
[Authorize(Roles = "staff,admin")]
public class StaffCustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public StaffCustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
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
}
