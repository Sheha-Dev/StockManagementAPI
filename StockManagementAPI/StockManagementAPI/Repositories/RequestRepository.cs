using Azure.Core;
using Microsoft.EntityFrameworkCore;
using StockManagementAPI.Data;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;
using StockManagementAPI.Exceptions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AppDbContext _db;

        public RequestRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<StockRequest>> GetAllAsync(Guid? requestStatusId = null, Guid? fromBranchId = null, Guid? toBranchId = null, Guid? productId = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.StockRequests
                .Include(r => r.FromBranch)
                .Include(r => r.ToBranch)
                .Include(r => r.RequestStatus)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Product)
                .AsQueryable();

            if (requestStatusId.HasValue)
                query = query.Where(r => r.RequestStatusId == requestStatusId.Value);

            if (fromBranchId.HasValue)
                query = query.Where(r => r.FromBranchId == fromBranchId.Value);

            if (toBranchId.HasValue)
                query = query.Where(r => r.ToBranchId == toBranchId.Value);

            if (productId.HasValue)
                query = query.Where(r => r.RequestItems.Any(ri => ri.ProductId == productId.Value));

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            return await query
                .OrderBy(r => r.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<StockRequest?> GetByIdAsync(Guid id)
        {
            return await _db.StockRequests
                .Include(r => r.FromBranch)
                .Include(r => r.ToBranch)
                .Include(r => r.RequestStatus)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Product)
                .Include(r => r.ReceivedItems)
                .FirstOrDefaultAsync(r => r.RequestId == id);
        }
        



        public async Task AddAsync(StockRequestDto dto)
        {
            await using var transaction =
                await _db.Database.BeginTransactionAsync();

            try
            {
                // Find the default request status
                var requestStatus = await _db.RequestStatuses
                    .FirstOrDefaultAsync(rs =>
                        rs.RequestStatusName == "Requested");

                if (requestStatus == null)
                {
                    throw new InvalidOperationException(
                        "Requested status was not found.");
                }

                // Create the stock request
                var request = new StockRequest
                {
                    RequestId = Guid.NewGuid(),
                    FromBranchId = dto.FromBranchId,
                    ToBranchId = dto.ToBranchId,
                    RequestStatusId = requestStatus.RequestStatusId,
                    CreatedBy = "Admin", // Replace with authenticated user
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                // Add the request to EF Core
                await _db.StockRequests.AddAsync(request);

                // Create request items
                foreach (var item in dto.RequestItems)
                {
                    var requestItem = new RequestItem
                    {
                        RequestItemId = Guid.NewGuid(),
                        RequestId = request.RequestId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        CreatedBy = "Admin", // Replace with authenticated user
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };

                    await _db.RequestItems.AddAsync(requestItem);
                }

                // Save request and items
                await _db.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(StockRequest entity)
        {
            _db.StockRequests.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task AddReceivedItemAsync(ReceivedItem item)
        {
            await _db.ReceivedItems.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task DispatchAsync(Guid requestId, string dispatchedBy)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var request = await GetByIdAsync(requestId);
                if (request == null) throw new APIException(404, "NOT_FOUND", "Request not found");

                if (!string.Equals(request.RequestStatus.RequestStatusName, "Requested", StringComparison.OrdinalIgnoreCase))
                    throw new APIException(400, "INVALID_STATUS", "Only requests in 'Requested' status can be dispatched");

                //// check availability
                //foreach (var item in request.RequestItems)
                //{
                //    var stock = await _db.BranchStocks.FirstOrDefaultAsync(bs => bs.BranchId == request.FromBranchId && bs.ProductId == item.ProductId);
                //    if (stock == null || stock.ProductQuantity < item.Quantity)
                //    {
                //        throw new APIException(400, "INSUFFICIENT_STOCK", $"Not enough stock for product {item.ProductId} in branch {request.FromBranchId}");
                //    }
                //}

                // deduct from source branch (reserve)
                foreach (var item in request.RequestItems)
                {
                    var stock = await _db.BranchStocks.FirstOrDefaultAsync(bs => bs.BranchId == request.FromBranchId && bs.ProductId == item.ProductId);
                    stock.ProductQuantity -= item.Quantity;
                    stock.UpdatedBy = dispatchedBy;
                    stock.UpdatedDate = DateTime.UtcNow;
                    _db.BranchStocks.Update(stock);
                }

                // set status to Dispatched
                var dispatchedStatus = await _db.RequestStatuses.FirstOrDefaultAsync(s => s.RequestStatusName == "Dispatched");
                if (dispatchedStatus == null) throw new APIException(500, "MISSING_STATUS", "Dispatched status not configured");
                request.RequestStatusId = dispatchedStatus.RequestStatusId;
                request.UpdatedBy = dispatchedBy;
                request.UpdatedDate = DateTime.Now;
                _db.StockRequests.Update(request);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task ReceiveAsync(Guid requestId, string receivedBy)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var request = await GetByIdAsync(requestId);
                if (request == null) throw new APIException(404, "NOT_FOUND", "Request not found");

                if (!string.Equals(request.RequestStatus.RequestStatusName, "Dispatched", StringComparison.OrdinalIgnoreCase))
                    throw new APIException(400, "INVALID_STATUS", "Only dispatched requests can be marked as received");

                // add to destination branch stock
                foreach (var item in request.RequestItems)
                {
                    var stock = await _db.BranchStocks.FirstOrDefaultAsync(bs => bs.BranchId == request.ToBranchId && bs.ProductId == item.ProductId);
                    if (stock == null)
                    {
                        stock = new BranchStock
                        {
                            BranchStockId = Guid.NewGuid(),
                            BranchId = request.ToBranchId,
                            ProductId = item.ProductId,
                            ProductQuantity = item.Quantity,
                            CreatedBy = receivedBy,
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };
                        await _db.BranchStocks.AddAsync(stock);
                    }
                    else
                    {
                        stock.ProductQuantity += item.Quantity;
                        stock.UpdatedBy = receivedBy;
                        stock.UpdatedDate = DateTime.Now;
                        _db.BranchStocks.Update(stock);
                    }

                    var receivedItem = new ReceivedItem
                    {
                        ReceivedItemId = Guid.NewGuid(),
                        RequestId = request.RequestId,
                        RequestItemId = item.RequestItemId,
                        DamagedQuantity = 0,
                        CreatedBy = receivedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    await _db.ReceivedItems.AddAsync(receivedItem);
                }

                // set status to Received
                var receivedStatus = await _db.RequestStatuses.FirstOrDefaultAsync(s => s.RequestStatusName == "Received");
                if (receivedStatus == null) throw new APIException(500, "MISSING_STATUS", "Received status not configured");
                request.RequestStatusId = receivedStatus.RequestStatusId;
                request.UpdatedBy = receivedBy;
                request.UpdatedDate = DateTime.Now;
                _db.StockRequests.Update(request);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task CancelAsync(Guid requestId, string cancelledBy)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var request = await GetByIdAsync(requestId);
                if (request == null) throw new APIException(404, "NOT_FOUND", "Request not found");

                if (!string.Equals(request.RequestStatus.RequestStatusName, "Requested", StringComparison.OrdinalIgnoreCase))
                    throw new APIException(400, "INVALID_STATUS", "Only requests in 'Requested' status can be cancelled");

                var cancelledStatus = await _db.RequestStatuses.FirstOrDefaultAsync(s => s.RequestStatusName == "Cancelled");
                if (cancelledStatus == null) throw new APIException(500, "MISSING_STATUS", "Cancelled status not configured");
                request.RequestStatusId = cancelledStatus.RequestStatusId;
                request.UpdatedBy = cancelledBy;
                request.UpdatedDate = DateTime.Now;
                _db.StockRequests.Update(request);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        

        

        

        
    }
}
