using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class Branch
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property ( To this )
        public ICollection<StockRequest> RequestFrom { get; set; } = new List<StockRequest>();
        public ICollection<StockRequest> RequestTo { get; set; } = new List<StockRequest>();
        public ICollection<BranchStock> BranchStocks { get; set; } = new List<BranchStock>();

    }
}
