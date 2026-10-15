using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchResponseDto>> GetAllAsync();
        Task<BranchResponseDto?> GetByIdAsync(Guid id);
    }
}
