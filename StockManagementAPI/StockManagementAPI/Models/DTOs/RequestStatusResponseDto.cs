using System;

namespace StockManagementAPI.Models.DTOs
{
    public class RequestStatusResponseDto
    {
        public Guid RequestStatusId { get; set; }
        public string RequestStatusName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
