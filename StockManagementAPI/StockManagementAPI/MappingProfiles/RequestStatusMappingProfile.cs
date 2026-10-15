using AutoMapper;
using StockManagementAPI.Models.DTOs;
using StockManagementAPI.Models.Entities;

namespace StockManagementAPI.MappingProfiles
{
    public class RequestStatusMappingProfile : Profile
    {
        public RequestStatusMappingProfile()
        {
            CreateMap<RequestStatus, RequestStatusResponseDto>()
                .ForMember(dest => dest.RequestStatusId, opt => opt.MapFrom(src => src.RequestStatusId))
                .ForMember(dest => dest.RequestStatusName, opt => opt.MapFrom(src => src.RequestStatusName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
