using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/customer-reports")]
    public class CustomerReportsController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public CustomerReportsController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerReports()
        {
            var reports = await _saleService.GetCustomerReportsAsync();

            return Ok(reports);
        }
    }
}