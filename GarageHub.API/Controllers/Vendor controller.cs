using GarageHub.Application.DTOs.Vendor;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vendors = await _vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null) return NotFound();
            return Ok(vendor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorDto dto)
        {
            var vendor = await _vendorService.CreateVendorAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVendorDto dto)
        {
            var vendor = await _vendorService.UpdateVendorAsync(id, dto);
            if (vendor == null) return NotFound();
            return Ok(vendor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _vendorService.DeleteVendorAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}