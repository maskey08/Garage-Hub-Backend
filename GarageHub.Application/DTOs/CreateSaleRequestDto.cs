using System.Collections.Generic;

namespace GarageHub.Application.DTOs
{
    public class CreateSaleRequestDto
    {
        public int CustomerId { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public List<SaleItemDto> Items { get; set; }
            = new List<SaleItemDto>();
    }
}