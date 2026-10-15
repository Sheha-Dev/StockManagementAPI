using AutoMapper;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockManagementAPI.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repo;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BranchResponseDto>> GetAllAsync()
        {
            var branches = await _repo.GetAllAsync();

            var dtos = _mapper.Map<IEnumerable<BranchResponseDto>>(branches);
            return dtos;
        }

        public async Task<BranchResponseDto?> GetByIdAsync(Guid id)
        {
            var branch = await _repo.GetByIdAsync(id);
            return branch is null ? null : _mapper.Map<BranchResponseDto>(branch);
        }
    }
}
