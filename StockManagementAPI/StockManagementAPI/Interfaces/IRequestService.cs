using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Interfaces
{
    public interface IRequestService
    {
        Task<IEnumerable<StockManagementAPI.Models.DTOs.StockReqResponseDto>> GetAllAsync(Guid? requestStatusId = null, Guid? fromBranchId = null, Guid? toBranchId = null, Guid? productId = null, int pageNumber = 1, int pageSize = 10);
        Task<StockManagementAPI.Models.DTOs.StockReqResponseDto?> GetByIdAsync(Guid id);
        Task AddAsync(StockManagementAPI.Models.DTOs.StockRequestDto dto, string createdBy);
        
    }
}
