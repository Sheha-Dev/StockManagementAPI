using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class Branch
    {
        public Guid BranchId { get; set; }
        [MaxLength(50)]
        public string BranchName { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
