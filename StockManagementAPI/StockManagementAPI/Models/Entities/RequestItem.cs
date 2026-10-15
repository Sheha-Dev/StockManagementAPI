using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class RequestItem
    {
        [Key]
        public Guid RequestItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid ProductId { get; set; }

        [Range(1, 10000, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
