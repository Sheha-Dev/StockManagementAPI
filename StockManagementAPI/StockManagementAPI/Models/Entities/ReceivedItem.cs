using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class ReceivedItem
    {
        public Guid ReceivedItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid RequestItemId { get; set; }
        public int DamagedQuantity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property 
        public RequestItem RequestItem { get; set; } = null!;

        public StockRequest Request  { get; set; } = null!;
    }
}
