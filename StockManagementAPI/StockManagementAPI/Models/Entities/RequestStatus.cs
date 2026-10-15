using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class RequestStatus
    {
        public Guid RequestStatusId { get; set; }
        public string RequestStatusName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property 
        public ICollection<StockRequest> Requests { get; set; } = new List<StockRequest>();
    }
}
