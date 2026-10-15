using System;
using System.Collections.Generic;

namespace StockManagementAPI.Models.DTOs
{
    public class StockRequestDto
    {
        public Guid FromBranchId { get; set; }
        public Guid ToBranchId { get; set; }

        public ICollection<StockRequestItemDto> RequestItems { get; set; } = new List<StockRequestItemDto>();
    }
}
