using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockManagementAPI.Data;
using StockManagementAPI.Exceptions;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;
using StockManagementAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagementAPI.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _repo;
        private readonly IBranchRepository _branchRepo;
        private readonly IProductRepository _productRepository;
        private readonly IRequestStatusRepository _requestStatusRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public RequestService(IRequestRepository repo, IBranchRepository branchRepo, IProductRepository productRepository, IRequestStatusRepository requestStatusRepo, IMapper mapper, AppDbContext db)
        {
            _repo = repo;
            _branchRepo = branchRepo;
            _productRepository = productRepository;
            _requestStatusRepo = requestStatusRepo;
            _mapper = mapper;
            _db = db;
        }

        public async Task AddAsync(StockRequestDto dto, string createdBy)
        {
            var statuses = await _requestStatusRepo.GetAllAsync();

            var requestedStatus = statuses?
                .FirstOrDefault(s => s.RequestStatusName == "Requested");

            var cancelStatus = statuses?
                .FirstOrDefault(s => s.RequestStatusName == "Cancelled");

            var branchName = ((Branch) await _branchRepo.GetByIdAsync(dto.FromBranchId)).BranchName;
            

            if (requestedStatus == null)
            {
                throw new APIException(404, "Not Found", "Request status not found.");
            }

            var requests = await _repo.GetAllAsync(
                        requestStatusId: requestedStatus.RequestStatusId,
                        fromBranchId: dto.FromBranchId
                    ) ?? Enumerable.Empty<StockRequest>();

            foreach (var item in dto.RequestItems)
            {
                var stock = await _branchRepo.GetBranchStockAsync(
                    dto.FromBranchId,
                    item.ProductId
                ) ?? new BranchStock();

                int reservedStock = requests
                    .SelectMany(r => r.RequestItems)
                    .Where(ri => ri.ProductId == item.ProductId )
                    .Sum(ri => ri.Quantity);


                var productName = ((Product)await _productRepository.GetByIdAsync(item.ProductId)).ProductName;

                int branchStockQty = (stock.ProductQuantity != null ? stock.ProductQuantity : 0);

                int availableQty = branchStockQty - reservedStock;

                if (stock == null || availableQty < item.Quantity)
                {
                    throw new APIException(
                        400,
                        "INSUFFICIENT_STOCK",
                        $"Not enough available stock for product {productName} in  {branchName}.The Available Qty is {availableQty}."
                    );
                }
            }

            await _repo.AddAsync(dto);
        }


        public async Task DispatchAsync(Guid requestId, string dispatchedBy)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var request = await _repo.GetByIdAsync(requestId);
                if (request == null) throw new APIException(404, "NOT_FOUND", "Request not found");

                if (!string.Equals(request.RequestStatus.RequestStatusName, "Requested", StringComparison.OrdinalIgnoreCase))
                    throw new APIException(400, "INVALID_STATUS", "Only requests in 'Requested' status can be dispatched");

                // check availability again
                foreach (var item in request.RequestItems)
                {
                    var stock = await _branchRepo.GetBranchStockAsync(request.FromBranchId, item.ProductId);
                    if (stock == null || stock.ProductQuantity < item.Quantity)
                    {
                        throw new APIException(400, "INSUFFICIENT_STOCK", $"Not enough stock for product {item.ProductId} in branch {request.FromBranchId}");
                    }
                }

                // deduct from source branch now (reserve)
                foreach (var item in request.RequestItems)
                {
                    var stock = await _branchRepo.GetBranchStockAsync(request.FromBranchId, item.ProductId);
                    stock.ProductQuantity -= item.Quantity;
                    stock.UpdatedBy = dispatchedBy;
                    stock.UpdatedDate = DateTime.UtcNow;
                    await _branchRepo.UpdateBranchStockAsync(stock);
                }

                // update status to Dispatched
                var dispatchedStatus = (await _requestStatusRepo.GetAllAsync()).FirstOrDefault(s => s.RequestStatusName.Equals("Dispatched", StringComparison.OrdinalIgnoreCase));
                if (dispatchedStatus == null) throw new APIException(500, "MISSING_STATUS", "Dispatched status not configured");
                request.RequestStatusId = dispatchedStatus.RequestStatusId;
                await _repo.UpdateAsync(request);

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
                var request = await _repo.GetByIdAsync(requestId);
                if (request == null) throw new APIException(404, "NOT_FOUND", "Request not found");

                if (!string.Equals(request.RequestStatus.RequestStatusName, "Dispatched", StringComparison.OrdinalIgnoreCase))
                    throw new APIException(400, "INVALID_STATUS", "Only dispatched requests can be marked as received");

                // add to destination branch stock
                foreach (var item in request.RequestItems)
                {
                    var stock = await _branchRepo.GetBranchStockAsync(request.ToBranchId, item.ProductId);
                    if (stock == null)
                    {
                        stock = new BranchStock
                        {
                            BranchStockId = Guid.NewGuid(),
                            BranchId = request.ToBranchId,
                            ProductId = item.ProductId,
                            ProductQuantity = item.Quantity,
                            CreatedBy = receivedBy,
                            CreatedDate = DateTime.UtcNow,
                            IsActive = true
                        };
                        await _branchRepo.AddBranchStockAsync(stock);
                    }
                    else
                    {
                        stock.ProductQuantity += item.Quantity;
                        stock.UpdatedBy = receivedBy;
                        stock.UpdatedDate = DateTime.UtcNow;
                        await _branchRepo.UpdateBranchStockAsync(stock);
                    }

                    // record received item
                    var receivedItem = new ReceivedItem
                    {
                        ReceivedItemId = Guid.NewGuid(),
                        RequestId = request.RequestId,
                        RequestItemId = item.RequestItemId,
                        DamagedQuantity = 0,
                        CreatedBy = receivedBy,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };
                    await _repo.AddReceivedItemAsync(receivedItem);
                }

                // update status to Received
                var receivedStatus = (await _requestStatusRepo.GetAllAsync()).FirstOrDefault(s => s.RequestStatusName.Equals("Received", StringComparison.OrdinalIgnoreCase));
                if (receivedStatus == null) throw new APIException(500, "MISSING_STATUS", "Received status not configured");
                request.RequestStatusId = receivedStatus.RequestStatusId;
                await _repo.UpdateAsync(request);

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
                var request = await _repo.GetByIdAsync(requestId);
                if (request == null) throw new APIException(404, "NOT_FOUND", "Request not found");

                if (!string.Equals(request.RequestStatus.RequestStatusName, "Requested", StringComparison.OrdinalIgnoreCase))
                    throw new APIException(400, "INVALID_STATUS", "Only requests in 'Requested' status can be cancelled");

                var cancelledStatus = (await _requestStatusRepo.GetAllAsync()).FirstOrDefault(s => s.RequestStatusName.Equals("Cancelled", StringComparison.OrdinalIgnoreCase));
                if (cancelledStatus == null) throw new APIException(500, "MISSING_STATUS", "Cancelled status not configured");
                request.RequestStatusId = cancelledStatus.RequestStatusId;
                await _repo.UpdateAsync(request);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }


        public async Task<IEnumerable<StockReqResponseDto>> GetAllAsync(Guid? requestStatusId = null, Guid? fromBranchId = null, Guid? toBranchId = null, Guid? productId = null, int pageNumber = 1, int pageSize = 10)
        {
            var entities = await _repo.GetAllAsync(requestStatusId, fromBranchId, toBranchId, productId, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<StockReqResponseDto>>(entities);
        }

        public async Task<StockReqResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<StockReqResponseDto>(entity);
        }

        
    }
}
