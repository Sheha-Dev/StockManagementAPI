using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Interfaces
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<Branch?> GetByIdAsync(Guid id);
        Task<BranchStock?> GetBranchStockAsync(Guid branchId, Guid productId);
        Task UpdateBranchStockAsync(BranchStock stock);
        Task AddBranchStockAsync(BranchStock stock);
    }
}
