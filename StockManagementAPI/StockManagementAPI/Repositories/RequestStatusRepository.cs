using Microsoft.EntityFrameworkCore;
using StockManagementAPI.Data;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Repositories
{
    public class RequestStatusRepository : IRequestStatusRepository
    {
        private readonly AppDbContext _db;

        public RequestStatusRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<RequestStatus>> GetAllAsync()
        {
            return await _db.RequestStatuses.ToListAsync();
        }

        public async Task<RequestStatus?> GetByIdAsync(Guid id)
        {
            return await _db.RequestStatuses.FindAsync(id);
        }
    }
}
