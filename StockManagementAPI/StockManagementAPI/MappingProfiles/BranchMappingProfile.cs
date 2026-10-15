using AutoMapper;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;

namespace StockManagementAPI.MappingProfiles
{
    public class BranchMappingProfile : Profile
    {
        public BranchMappingProfile()
        {
            CreateMap<Branch, BranchResponseDto>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }

    }
}
