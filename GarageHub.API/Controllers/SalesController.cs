using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "staff,admin")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IEmailService _emailService;

        public SalesController(ISaleService saleService, IEmailService emailService)
        {
            _saleService = saleService;
            _emailService = emailService;
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

        [HttpPost("send-invoice-email/{saleId}")]
        public async Task<IActionResult> SendInvoiceEmail(int saleId)
        {
            var email = "test@gmail.com"; // change later

            var subject = "GarageHub Invoice";

            var body = $"<h3>Your invoice (Sale ID: {saleId}) has been generated.</h3>";

            await _emailService.SendInvoiceEmailAsync(email, subject, body);

            return Ok("Email sent successfully");
        }


        [HttpGet("users/{id}/loyalty")]
        public async Task<IActionResult> GetLoyalty(int id)
        {
            try
            {
                var result = await _saleService.GetLoyaltyPointsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}