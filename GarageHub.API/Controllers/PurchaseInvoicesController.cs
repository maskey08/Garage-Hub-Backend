using GarageHub.Application.DTOs.PurchaseInvoice;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Domain.Enums;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/purchase-invoices")]
[Authorize(Roles = "admin,staff")]
public class PurchaseInvoicesController : ControllerBase
{
    private readonly IPurchaseInvoiceService _invoiceService;
    private readonly IEmailService _emailService;
    private readonly AppDbContext _db;

    public PurchaseInvoicesController(IPurchaseInvoiceService invoiceService, IEmailService emailService, AppDbContext db)
    {
        _invoiceService = invoiceService;
        _emailService = emailService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.Trim().ToLowerInvariant();
            invoices = invoices.Where(i =>
                i.InvoiceNumber.ToLowerInvariant().Contains(lowered)
                || (i.VendorName != null && i.VendorName.ToLowerInvariant().Contains(lowered))
                || i.Id.ToString().Contains(lowered));
        }

        return Ok(invoices);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        return invoice == null ? NotFound() : Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseInvoiceDto dto)
    {
        var invoice = await _invoiceService.CreateInvoiceAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    [HttpPatch("{id:int}/paid")]
    public async Task<IActionResult> MarkPaid(int id)
    {
        var updated = await _invoiceService.MarkAsPaidAsync(id);
        return updated ? Ok() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _invoiceService.DeleteInvoiceAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/email")]
    public async Task<IActionResult> EmailPurchaseInvoice(int id)
    {
        var invoice = await _db.PurchaseInvoices
            .Include(i => i.Vendor)
            .Include(i => i.Items)
            .ThenInclude(i => i.Part)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound(new { message = "Purchase invoice not found." });

        if (invoice.Vendor == null || string.IsNullOrWhiteSpace(invoice.Vendor.Email))
            return BadRequest(new { message = "Vendor does not have an email address." });

        var rows = string.Join("", invoice.Items.Select(item =>
            $"<tr><td>{item.Part?.PartName}</td><td>{item.Quantity}</td><td>{item.UnitPrice:N2}</td><td>{item.TotalPrice:N2}</td></tr>"));

        await _emailService.SendInvoiceEmailAsync(
            invoice.Vendor.Email,
            $"GarageHub Purchase Invoice {invoice.InvoiceNumber}",
            $"<h2>Purchase Invoice {invoice.InvoiceNumber}</h2><p>Vendor: {invoice.Vendor.Name}</p><table border='1' cellpadding='8' cellspacing='0'><tr><th>Part</th><th>Qty</th><th>Unit Price</th><th>Total</th></tr>{rows}</table><p><strong>Total:</strong> {invoice.TotalAmount:N2}</p>");

        var recipients = await _db.Users
            .Where(u => u.Role == "admin" || u.Role == "staff")
            .Select(u => u.UserId)
            .ToListAsync();
        foreach (var userId in recipients)
        {
            _db.Notifications.Add(new Notification
            {
                UserId = userId,
                Title = "Purchase invoice email sent",
                Message = $"{invoice.InvoiceNumber} was emailed to {invoice.Vendor.Email}.",
                Type = NotificationType.NewPurchaseInvoice,
                CreatedAt = DateTime.UtcNow
            });
        }
        await _db.SaveChangesAsync();

        return Ok(new { message = "Email sent successfully" });
    }
}
