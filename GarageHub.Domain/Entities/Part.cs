using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities
{
    public class Part
    {
        public int Id { get; set; }

        public string PartName { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }
}
