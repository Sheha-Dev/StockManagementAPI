using System;

namespace StockManagementAPI.Models.DTOs
{
    public class StockRequestItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
