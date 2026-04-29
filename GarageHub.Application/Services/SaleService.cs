using System;
using System.Threading.Tasks;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaleResponseDto> CreateSaleAsync(CreateSaleRequestDto request)
        {
            decimal subTotal = 0;

            var sale = new Sale
            {
                CustomerId = request.CustomerId,
                PaymentMethod = request.PaymentMethod,
                SaleDate = DateTime.UtcNow
            };

            foreach (var item in request.Items)
            {
                var part = await _context.Parts
                    .FirstOrDefaultAsync(p => p.Id == item.PartId);

                if (part == null)
                    throw new Exception($"Part with ID {item.PartId} not found.");

                if (part.StockQuantity < item.Quantity)
                    throw new Exception($"Not enough stock for {part.PartName}");

                decimal totalPrice = part.Price * item.Quantity;

                subTotal += totalPrice;

                part.StockQuantity -= item.Quantity;

                sale.SaleItems.Add(new SaleItem
                {
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    UnitPrice = part.Price,
                    TotalPrice = totalPrice
                });
            }

            decimal taxAmount = subTotal * 0.18m;
            decimal grandTotal = subTotal + taxAmount;

            sale.SubTotal = subTotal;
            sale.TaxAmount = taxAmount;
            sale.GrandTotal = grandTotal;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var invoice = new Invoice
            {
                SaleId = sale.Id,
                InvoiceNumber = $"INV-{DateTime.UtcNow.Ticks}"
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return new SaleResponseDto
            {
                SaleId = sale.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                GrandTotal = grandTotal,
                SaleDate = sale.SaleDate
            };
        }
    }
}