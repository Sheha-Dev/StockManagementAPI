using AutoMapper;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Services
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly IRequestStatusRepository _repo;
        private readonly IMapper _mapper;

        public RequestStatusService(IRequestStatusRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RequestStatusResponseDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<RequestStatusResponseDto>>(items);
        }

        public async Task<RequestStatusResponseDto?> GetByIdAsync(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item is null ? null : _mapper.Map<RequestStatusResponseDto>(item);
        }
    }
}
