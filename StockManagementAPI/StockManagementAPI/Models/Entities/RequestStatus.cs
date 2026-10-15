using System.ComponentModel.DataAnnotations;

namespace StockManagementAPI.Models.Entities
{
    public class RequestStatus
    {
        public Guid RequestStatusId { get; set; }
        [MaxLength(50)]
        public string RequestStatus { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
