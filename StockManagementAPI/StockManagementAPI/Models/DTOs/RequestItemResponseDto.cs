using System;

namespace StockManagementAPI.Models.DTOs
{
    public class RequestItemResponseDto
    {
        public Guid RequestItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
    }
}
