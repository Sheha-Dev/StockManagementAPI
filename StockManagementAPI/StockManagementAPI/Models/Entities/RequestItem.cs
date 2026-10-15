using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class RequestItem
    {
        public Guid RequestItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property 
        public Product Product { get; set; } = null!;

        public StockRequest Request { get; set; } = null!;

        public ICollection<ReceivedItem> ReceivedItems { get; set; } = new List<ReceivedItem>();

    }
}
