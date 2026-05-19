using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Infrastructure.Data;
using GarageHub.Domain.Entities;
using GarageHub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace GarageHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "staff,admin")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _db;

        public SalesController(ISaleService saleService, IEmailService emailService, AppDbContext db)
        {
            _saleService = saleService;
            _emailService = emailService;
            _db = db;
        }

        [HttpGet("/api/invoices")]
        public async Task<IActionResult> GetInvoices([FromQuery] string? status, [FromQuery] string? search)
        {
            var query = _db.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .ThenInclude(i => i.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(s => s.PaymentStatus.ToLower() == status.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(s =>
                    s.SaleId.ToString().Contains(term) ||
                    s.Customer.Email.ToLower().Contains(term) ||
                    s.Customer.FirstName.ToLower().Contains(term) ||
                    s.Customer.LastName.ToLower().Contains(term));
            }

            var invoices = await query
                .OrderByDescending(s => s.SaleDate)
                .Select(s => new
                {
                    id = s.SaleId.ToString(),
                    date = s.SaleDate.ToString("yyyy-MM-dd"),
                    customerName = (s.Customer.FirstName + " " + s.Customer.LastName).Trim(),
                    initials = ((s.Customer.FirstName == "" ? "C" : s.Customer.FirstName.Substring(0, 1)) +
                                (s.Customer.LastName == "" ? "" : s.Customer.LastName.Substring(0, 1))).ToUpper(),
                    amount = s.TotalAmount,
                    status = string.IsNullOrWhiteSpace(s.PaymentStatus) ? "pending" : s.PaymentStatus,
                    itemCount = s.Items.Count,
                    invoiceNumber = "INV-" + s.SaleId
                })
                .ToListAsync();

            return Ok(invoices);
        }

        [HttpGet("/api/invoices/{saleId:int}")]
        public async Task<IActionResult> GetInvoice(int saleId)
        {
            var invoice = await _db.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(s => s.SaleId == saleId);

            return invoice == null ? NotFound() : Ok(invoice);
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
            var invoice = await _db.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(s => s.SaleId == saleId);

            if (invoice == null)
                return NotFound(new { message = "Invoice not found." });

            if (string.IsNullOrWhiteSpace(invoice.Customer.Email))
                return BadRequest(new { message = "Customer does not have an email address." });

            var subject = "GarageHub Invoice";

            var rows = string.Join("", invoice.Items.Select(item =>
                $"<tr><td>{item.Part.PartName}</td><td>{item.Quantity}</td><td>{item.UnitPrice:N2}</td><td>{item.TotalPrice:N2}</td></tr>"));

            var body = $@"
                <h2>GarageHub Invoice INV-{invoice.SaleId}</h2>
                <p>Hello {invoice.Customer.FirstName}, your invoice has been generated.</p>
                <table border='1' cellpadding='8' cellspacing='0'>
                    <thead><tr><th>Part</th><th>Qty</th><th>Unit Price</th><th>Total</th></tr></thead>
                    <tbody>{rows}</tbody>
                </table>
                <p><strong>Subtotal:</strong> {invoice.Subtotal:N2}</p>
                <p><strong>Discount:</strong> {invoice.DiscountApplied:N2}</p>
                <p><strong>Grand Total:</strong> {invoice.TotalAmount:N2}</p>";

            await _emailService.SendInvoiceEmailAsync(invoice.Customer.Email, subject, body);

            var recipients = await _db.Users
                .Where(u => u.Role == "admin" || u.Role == "staff")
                .Select(u => u.UserId)
                .ToListAsync();
            foreach (var userId in recipients)
            {
                _db.Notifications.Add(new Notification
                {
                    UserId = userId,
                    Title = "Invoice email sent",
                    Message = $"Invoice INV-{invoice.SaleId} was sent to {invoice.Customer.Email}.",
                    Type = NotificationType.GeneralAlert,
                    CreatedAt = DateTime.UtcNow
                });
            }
            await _db.SaveChangesAsync();

            return Ok(new { message = "Email sent successfully" });
        }

        [HttpPost("/api/invoices/{saleId:int}/email")]
        public Task<IActionResult> EmailInvoice(int saleId) => SendInvoiceEmail(saleId);

        [HttpGet("/api/invoices/{saleId:int}/download")]
        public async Task<IActionResult> DownloadInvoice(int saleId)
        {
            var invoice = await _db.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(s => s.SaleId == saleId);

            if (invoice == null)
                return NotFound();

            var lines = new List<string>
            {
                $"GarageHub Invoice INV-{invoice.SaleId}",
                $"Date: {invoice.SaleDate:yyyy-MM-dd HH:mm}",
                $"Customer: {(invoice.Customer.FirstName + " " + invoice.Customer.LastName).Trim()}",
                "",
                "Items:"
            };
            lines.AddRange(invoice.Items.Select(i => $"{i.Part.PartName} x {i.Quantity} @ {i.UnitPrice:N2} = {i.TotalPrice:N2}"));
            lines.Add("");
            lines.Add($"Subtotal: {invoice.Subtotal:N2}");
            lines.Add($"Discount: {invoice.DiscountApplied:N2}");
            lines.Add($"Total: {invoice.TotalAmount:N2}");

            var bytes = System.Text.Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, lines));
            return File(bytes, "text/plain", $"INV-{invoice.SaleId}.txt");
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
