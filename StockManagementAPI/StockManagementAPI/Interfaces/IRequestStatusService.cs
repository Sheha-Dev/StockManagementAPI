using StockManagementAPI.Models.Entities;
using StockManagementAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Interfaces
{
    public interface IRequestStatusService
    {
        Task<IEnumerable<RequestStatusResponseDto>> GetAllAsync();
        Task<RequestStatusResponseDto?> GetByIdAsync(Guid id);
        // Create/Update/Delete removed - service exposes only read methods for request statuses
    }
}
