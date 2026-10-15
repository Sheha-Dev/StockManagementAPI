using AutoMapper;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;

namespace StockManagementAPI.MappingProfiles
{
    public class RequestMappingProfile : Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<RequestItem, RequestItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

            // Reverse maps for creating requests from DTOs
            CreateMap<StockRequestItemDto, RequestItem>()
                .ForMember(dest => dest.RequestItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<StockRequestDto, StockRequest>()
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RequestItems, opt => opt.MapFrom(src => src.RequestItems));

            CreateMap<StockRequest, StockReqResponseDto>()
                .ForMember(dest => dest.FromBranchName, opt => opt.MapFrom(src => src.FromBranch.BranchName))
                .ForMember(dest => dest.ToBranchName, opt => opt.MapFrom(src => src.ToBranch.BranchName))
                .ForMember(dest => dest.RequestStatusName, opt => opt.MapFrom(src => src.RequestStatus.RequestStatusName))
                .ForMember(dest => dest.RequestItems, opt => opt.MapFrom(src => src.RequestItems));
        }
    }
}
