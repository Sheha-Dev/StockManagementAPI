namespace StockManagementAPI.Models.Entities
{
    public class BranchStock
    {
        public Guid BranchStockId { get; set; }
        public Guid BranchId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property 
        public Branch Branch { get; set; } = null!;
        public Product Product { get; set; } = null!;
        
    }
}

