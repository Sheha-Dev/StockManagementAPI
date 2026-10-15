using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class ReceivedItem
    {
        public Guid ReceivedItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid RequestItemId { get; set; }
        [Range(1, 10000, ErrorMessage = "Quantity must be at least 1.")]
        public int DamagedQuantity { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
