using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class Request
    {
        [Key]
        public Guid RequestId { get; set; }
        public Guid FromBranchId { get; set; }
        public Guid ToBranchId { get; set; }
        public Guid RequestStatusId { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
