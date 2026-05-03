using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GarageHub.Application.Interfaces;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // Feature 10: Staff search customers by name, phone, id
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> SearchCustomers([FromQuery] string searchTerm, [FromQuery] string searchBy)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        if (string.IsNullOrWhiteSpace(searchBy))
            return BadRequest("Search by parameter is required (name, phone, id)");

        var customers = await _customerService.SearchCustomersAsync(searchTerm, searchBy);
        return Ok(customers);
    }

    // Feature 8: Staff view customer details with history
    [HttpGet("{id}/details")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetCustomerWithDetails(int id)
    {
        var customer = await _customerService.GetCustomerWithDetailsAsync(id);
        if (customer == null)
            return NotFound($"Customer with ID {id} not found");

        return Ok(customer);
    }
}