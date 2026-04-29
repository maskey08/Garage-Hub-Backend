using Microsoft.AspNetCore.Mvc;
using GarageHub.Application.Interfaces;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found");

            return Ok(customer);
        }

        // GET: api/customer/{id}/details (Feature 8 - customer history)
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetCustomerWithDetails(int id)
        {
            var customer = await _customerService.GetCustomerWithDetailsAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found");

            return Ok(customer);
        }

        // GET: api/customer/search?searchTerm=john&searchBy=name (Feature 10)
        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers([FromQuery] string searchTerm, [FromQuery] string searchBy)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term is required");

            if (string.IsNullOrWhiteSpace(searchBy))
                return BadRequest("Search by parameter is required (name, phone, id, vehicleNumber)");

            var customers = await _customerService.SearchCustomersAsync(searchTerm, searchBy);
            return Ok(customers);
        }
    }
}