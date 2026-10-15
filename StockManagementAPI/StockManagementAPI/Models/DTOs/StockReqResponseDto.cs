using System;
using System.Collections.Generic;

namespace StockManagementAPI.Models.DTOs
{
    public class StockReqResponseDto
    {
        public Guid RequestId { get; set; }
        public Guid FromBranchId { get; set; }
        public string FromBranchName { get; set; } = string.Empty;
        public Guid ToBranchId { get; set; }
        public string ToBranchName { get; set; } = string.Empty;
        public Guid RequestStatusId { get; set; }
        public string RequestStatusName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public ICollection<RequestItemResponseDto> RequestItems { get; set; } = new List<RequestItemResponseDto>();
    }
}
