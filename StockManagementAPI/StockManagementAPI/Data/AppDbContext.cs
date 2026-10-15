
using Microsoft.EntityFrameworkCore;
using StockManagementAPI.Models.Entities;

namespace StockManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Database tables
        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<RequestStatus> RequestStatuses { get; set; } = null!;
        public DbSet<StockRequest> StockRequests { get; set; } = null!;
        public DbSet<RequestItem> RequestItems { get; set; } = null!;
        public DbSet<ReceivedItem> ReceivedItems { get; set; } = null!;
        public DbSet<BranchStock> BranchStocks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================
            // 1. BRANCH
            // =====================================
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(b => b.BranchId);

                entity.Property(b => b.BranchName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(b => b.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(b => b.CreatedDate)
                    .IsRequired();

                entity.Property(b => b.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(b => b.UpdatedDate)
                    .IsRequired(false);

                entity.Property(b => b.IsActive)
                    .IsRequired();
            });

            // =====================================
            // 2. PRODUCT
            // =====================================
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.CreatedDate)
                    .IsRequired();

                entity.Property(p => p.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(p => p.UpdatedDate)
                    .IsRequired(false);

                entity.Property(p => p.IsActive)
                    .IsRequired();
            });

            // =====================================
            // 3. REQUEST STATUS
            // =====================================
            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.HasKey(s => s.RequestStatusId);

                entity.Property(s => s.RequestStatusName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.CreatedDate)
                    .IsRequired();

                entity.Property(s => s.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(s => s.UpdatedDate)
                    .IsRequired(false);

                entity.Property(s => s.IsActive)
                    .IsRequired();
            });

            // =====================================
            // 4. REQUEST
            // =====================================
            modelBuilder.Entity<StockRequest>(entity =>
            {
                entity.ToTable("Requests");
                entity.HasKey(r => r.RequestId);

                entity.Property(r => r.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(r => r.CreatedDate)
                    .IsRequired();

                entity.Property(r => r.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(r => r.UpdatedDate)
                    .IsRequired(false);

                entity.Property(r => r.IsActive)
                    .IsRequired();

                // From Branch relationship
                entity.HasOne(r => r.FromBranch)
                    .WithMany(b => b.RequestFrom)
                    .HasForeignKey(r => r.FromBranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                // To Branch relationship
                entity.HasOne(r => r.ToBranch)
                    .WithMany(b => b.RequestTo)
                    .HasForeignKey(r => r.ToBranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Request Status relationship
                entity.HasOne(r => r.RequestStatus)
                    .WithMany(s => s.Requests)
                    .HasForeignKey(r => r.RequestStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =====================================
            // 5. REQUEST ITEM
            // =====================================
            modelBuilder.Entity<RequestItem>(entity =>
            {
                entity.HasKey(ri => ri.RequestItemId);

                entity.Property(ri => ri.Quantity)
                    .IsRequired();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_RequestItem_Quantity",
                        "[Quantity] >= 1"));

                entity.Property(ri => ri.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(ri => ri.CreatedDate)
                    .IsRequired();

                entity.Property(ri => ri.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(ri => ri.UpdatedDate)
                    .IsRequired(false);

                entity.Property(ri => ri.IsActive)
                    .IsRequired();

                // RequestItem -> Request
                entity.HasOne(ri => ri.Request)
                    .WithMany(r => r.RequestItems)
                    .HasForeignKey(ri => ri.RequestId)
                    .OnDelete(DeleteBehavior.Restrict);

                // RequestItem -> Product
                entity.HasOne(ri => ri.Product)
                    .WithMany(p => p.RequestItems)
                    .HasForeignKey(ri => ri.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =====================================
            // 6. RECEIVED ITEM
            // =====================================
            modelBuilder.Entity<ReceivedItem>(entity =>
            {
                entity.HasKey(ri => ri.ReceivedItemId);

                entity.Property(ri => ri.DamagedQuantity)
                    .IsRequired();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_ReceivedItem_DamagedQuantity",
                        "[DamagedQuantity] >= 0"));

                entity.Property(ri => ri.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(ri => ri.CreatedDate)
                    .IsRequired();

                entity.Property(ri => ri.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(ri => ri.UpdatedDate)
                    .IsRequired(false);

                entity.Property(ri => ri.IsActive)
                    .IsRequired();

                // ReceivedItem -> Request
                entity.HasOne(ri => ri.Request)
                    .WithMany(r => r.ReceivedItems)
                    .HasForeignKey(ri => ri.RequestId)
                    .OnDelete(DeleteBehavior.Restrict);

                // ReceivedItem -> RequestItem
                entity.HasOne(ri => ri.RequestItem)
                    .WithMany(r => r.ReceivedItems)
                    .HasForeignKey(ri => ri.RequestItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =====================================
            // 6. RECEIVED ITEM
            // =====================================
            modelBuilder.Entity<BranchStock>(entity =>
            {
                entity.ToTable("BranchStock");
                entity.HasKey(brs => brs.BranchStockId);

                entity.Property(brs => brs.ProductQuantity)
                    .IsRequired();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_BranchStock_ProductQuantity",
                        "[ProductQuantity] >= 0"));

                entity.Property(brs => brs.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(brs => brs.CreatedDate)
                    .IsRequired();

                entity.Property(brs => brs.UpdatedBy)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(brs => brs.UpdatedDate)
                    .IsRequired(false);

                entity.Property(brs => brs.IsActive)
                    .IsRequired();

                // BranchStock -> Branch
                entity.HasOne(brs => brs.Branch)
                    .WithMany(b => b.BranchStocks)
                    .HasForeignKey(brs => brs.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                // BranchStock -> Product
                entity.HasOne(brs => brs.Product)
                    .WithMany(p => p.BranchStocks)
                    .HasForeignKey(brs => brs.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
                    
    }
}
