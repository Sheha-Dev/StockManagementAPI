using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class Branch
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property ( To this )
        public ICollection<Request> RequestFrom { get; set; } = new List<Request>();
        public ICollection<Request> RequestTo { get; set; } = new List<Request>();

    }
}
