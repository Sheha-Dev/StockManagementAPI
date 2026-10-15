using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Interfaces
{
    public interface IRequestStatusRepository
    {
        Task<IEnumerable<RequestStatus>> GetAllAsync();
        Task<RequestStatus?> GetByIdAsync(Guid id);
    }
}
