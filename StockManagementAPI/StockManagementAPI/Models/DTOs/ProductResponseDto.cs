using System;

namespace StockManagementAPI.Models.DTOs
{
    public class ProductResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
