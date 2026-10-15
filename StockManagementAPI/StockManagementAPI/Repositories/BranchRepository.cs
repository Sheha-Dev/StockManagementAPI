using Microsoft.EntityFrameworkCore;
using StockManagementAPI.Data;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly AppDbContext _db;

        public BranchRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _db.Branches.ToListAsync();
        }

        public async Task<Branch?> GetByIdAsync(Guid id)
        {
            return await _db.Branches.FindAsync(id);
        }

        public async Task<BranchStock?> GetBranchStockAsync(Guid branchId, Guid productId)
        {
            return await _db.BranchStocks.FirstOrDefaultAsync(bs => bs.BranchId == branchId && bs.ProductId == productId);
        }

        public async Task UpdateBranchStockAsync(BranchStock stock)
        {
            _db.BranchStocks.Update(stock);
            await _db.SaveChangesAsync();
        }

        public async Task AddBranchStockAsync(BranchStock stock)
        {
            await _db.BranchStocks.AddAsync(stock);
            await _db.SaveChangesAsync();
        }
    }
}
