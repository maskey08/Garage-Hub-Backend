using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using GarageHub.Infrastructure.Repositories;

namespace GarageHub.Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly IPartRepository _partRepository;
        private readonly AppDbContext _context;

        public SaleService(IPartRepository partRepository, AppDbContext context)
        {
            _partRepository = partRepository;
            _context = context;
        }



        public async Task<SaleResponseDto> CreateSaleAsync(CreateSaleRequestDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); 

            try
            {
                decimal subTotal = 0;

                int pointsUsed = 0;
                int pointsEarned = 0;

                var sale = new SalesInvoice
                {
                    CustomerId = request.CustomerId,
                    SaleDate = DateTime.UtcNow
                };

                foreach (var item in request.Items)
                {
                    var part = await _partRepository.GetPartByIdAsync(item.PartId);

                    if (part == null)
                        throw new InvalidOperationException($"Part with ID {item.PartId} not found."); 

                    if (part.StockQuantity < item.Quantity)
                        throw new InvalidOperationException($"Not enough stock for {part.PartName}"); 

                    decimal totalPrice = part.Price * item.Quantity;
                    subTotal += totalPrice;

                    // reduce stock
                    part.StockQuantity -= item.Quantity;

                    await _partRepository.UpdatePartAsync(part);

                    sale.Items.Add(new SalesInvoiceItem
                    {
                        PartId = item.PartId,
                        Quantity = item.Quantity,
                        UnitPrice = (float)part.Price,
                    });
                }

                decimal taxAmount = subTotal * 0.18m;
                decimal grandTotal = subTotal + taxAmount;


                //Redeem loyalty logic
                var user = await _context.Users.FindAsync(request.CustomerId);

                int pointsToRedeem = request.PointsToRedeem;

                if (user != null && pointsToRedeem > 0)
                {
                    if (pointsToRedeem > user.LoyaltyPoints)
                        throw new InvalidOperationException("Not enough loyalty points");

                    if (pointsToRedeem > grandTotal)
                        pointsToRedeem = (int)grandTotal;

                    pointsUsed = pointsToRedeem;

                    grandTotal -= pointsToRedeem;

                    user.LoyaltyPoints -= pointsToRedeem;
                }

                sale.Subtotal = subTotal;
                sale.TotalAmount = grandTotal;

                // earn Loyalty Logic

                if (user != null)
                {
                    user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);

                    pointsEarned = (int)(grandTotal / 100);
                    user.LoyaltyPoints += pointsEarned;

                    _context.Users.Update(user);
                }

                // Save everything ONCE
                _context.SalesInvoices.Add(sale);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); 

                return new SaleResponseDto
                {
                    SaleId = sale.SaleId,
                    InvoiceNumber = $"INV-{sale.SaleId}",
                    SubTotal = subTotal,
                    TaxAmount = taxAmount,
                    GrandTotal = grandTotal,
                    SaleDate = sale.SaleDate,
                    PointsUsed = pointsUsed,
                    PointsEarned = pointsEarned
                };
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }

        public async Task<LoyaltyResponseDto> GetLoyaltyPointsAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            return new LoyaltyResponseDto
            {
                UserId = user.UserId,
                LoyaltyPoints = user.LoyaltyPoints
            };
        }

        public async Task<List<CustomerReportDto>> GetCustomerReportsAsync()
        {
            var users = await _context.Users.ToListAsync();
            var invoices = await _context.SalesInvoices.ToListAsync();

            var reports = users
                .Select(u => new CustomerReportDto
                {
                    CustomerId = u.UserId,
                    Email = u.Email,
                    LoyaltyPoints = u.LoyaltyPoints,

                    TotalInvoices = invoices
                        .Count(s => s.CustomerId == u.UserId),

                    TotalSpent = invoices
                        .Where(s => s.CustomerId == u.UserId)
                        .Sum(s => s.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .ToList();

            return reports;
        }
    }
}