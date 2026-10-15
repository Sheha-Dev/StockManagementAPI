using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<StockRequest>> GetAllAsync(Guid? requestStatusId = null, Guid? fromBranchId = null, Guid? toBranchId = null, Guid? productId = null, int pageNumber = 1, int pageSize = 10);
        Task<StockRequest?> GetByIdAsync(Guid id);
        Task AddAsync(StockRequestDto dto);
        Task UpdateAsync(StockRequest entity);
        Task AddReceivedItemAsync(ReceivedItem item);

        Task DispatchAsync(Guid requestId, string dispatchedBy);
        Task ReceiveAsync(Guid requestId, string receivedBy);
        Task CancelAsync(Guid requestId, string cancelledBy);

        
    }
}
