namespace StockManagementAPI.Models.DTOs
{
    public class BranchResponseDto
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
