using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class Request
    {

        public Guid RequestId { get; set; }
        public Guid FromBranchId { get; set; }
        public Guid ToBranchId { get; set; }
        public Guid RequestStatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property ( From this)

        public Branch FromBranch { get; set; } = null! ;
        public Branch ToBranch { get; set; } = null!;
        public RequestStatus RequestStatus { get; set; } = null!;

        // Navigation Property ( To this)
        public ICollection<RequestItem> RequestItems { get; set; } = new List<RequestItem>();
        public ICollection<ReceivedItem> ReceivedItems { get; set; } = new List<ReceivedItem>();
    }
}
