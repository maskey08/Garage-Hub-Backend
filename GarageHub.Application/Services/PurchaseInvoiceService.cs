using GarageHub.Application.DTOs.PurchaseInvoice;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Services
{
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private readonly IRepository<PurchaseInvoice> _invoiceRepository;
        private readonly IRepository<Part> _partRepository;

        public PurchaseInvoiceService(IRepository<PurchaseInvoice> invoiceRepository, IRepository<Part> partRepository)
        {
            _invoiceRepository = invoiceRepository;
            _partRepository = partRepository;
        }

        public async Task<IEnumerable<PurchaseInvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return invoices.Select(i => new PurchaseInvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                VendorId = i.VendorId,
                VendorName = i.Vendor?.Name,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount,
                IsPaid = i.IsPaid,
                CreatedAt = i.CreatedAt,
                Items = i.Items.Select(item => new PurchaseInvoiceItemDto
                {
                    Id = item.Id,
                    PartId = item.PartId,
                    PartName = item.Part?.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            });
        }

        public async Task<PurchaseInvoiceDto?> GetInvoiceByIdAsync(int id)
        {
            var i = await _invoiceRepository.GetByIdAsync(id);
            if (i == null) return null;
            return new PurchaseInvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                VendorId = i.VendorId,
                VendorName = i.Vendor?.Name,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount,
                IsPaid = i.IsPaid,
                CreatedAt = i.CreatedAt,
                Items = i.Items.Select(item => new PurchaseInvoiceItemDto
                {
                    Id = item.Id,
                    PartId = item.PartId,
                    PartName = item.Part?.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };
        }

        public async Task<PurchaseInvoiceDto> CreateInvoiceAsync(CreatePurchaseInvoiceDto dto)
        {
            var invoice = new PurchaseInvoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                VendorId = dto.VendorId,
                InvoiceDate = dto.InvoiceDate,
                Items = dto.Items.Select(item => new PurchaseInvoiceItem
                {
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
            invoice.TotalAmount = invoice.Items.Sum(i => i.Quantity * i.UnitPrice);

            // Update stock for each part
            foreach (var item in dto.Items)
            {
                var part = await _partRepository.GetByIdAsync(item.PartId);
                if (part != null)
                {
                    part.QuantityInStock += item.Quantity;
                    await _partRepository.UpdateAsync(part);
                }
            }

            await _invoiceRepository.AddAsync(invoice);
            return new PurchaseInvoiceDto { Id = invoice.Id, InvoiceNumber = invoice.InvoiceNumber };
        }

        public async Task<bool> MarkAsPaidAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return false;
            invoice.IsPaid = true;
            await _invoiceRepository.UpdateAsync(invoice);
            return true;
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return false;
            await _invoiceRepository.DeleteAsync(invoice);
            return true;
        }
    }
}