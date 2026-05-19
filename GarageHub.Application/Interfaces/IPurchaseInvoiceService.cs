using GarageHub.Application.DTOs.PurchaseInvoice;

namespace GarageHub.Application.Interfaces
{
    public interface IPurchaseInvoiceService
    {
        Task<IEnumerable<PurchaseInvoiceDto>> GetAllInvoicesAsync();
        Task<PurchaseInvoiceDto?> GetInvoiceByIdAsync(int id);
        Task<PurchaseInvoiceDto> CreateInvoiceAsync(CreatePurchaseInvoiceDto dto);
        Task<bool> MarkAsPaidAsync(int id);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
