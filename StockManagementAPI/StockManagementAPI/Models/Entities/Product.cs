using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [MaxLength(50)]
        public string ProductName { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
