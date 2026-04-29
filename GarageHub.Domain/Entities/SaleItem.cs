namespace GarageHub.Domain.Entities
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public int PartId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public Sale Sale { get; set; }
    }
}