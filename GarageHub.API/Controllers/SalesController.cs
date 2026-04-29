using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale(
            [FromBody] CreateSaleRequestDto request)
        {
            if (request == null || request.Items.Count == 0)
            {
                return BadRequest("Sale items are required.");
            }

            var result = await _saleService.CreateSaleAsync(request);

            return Ok(result);
        }
    }
}