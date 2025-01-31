using Application.DTOs.RoomClass;
using AutoMapper;
using Domain.Entities;
using Domain.Models;


namespace Application.Mapping
{
    public class RoomClassProfile : Profile
    {
        public RoomClassProfile()
        {
            CreateMap<RoomClassUpdateRequest, RoomClass>();
            CreateMap<RoomClassCreationRequest, RoomClass>();
            CreateMap<RoomClass, RoomClassForManagementResponse>();
            CreateMap<PaginatedList<RoomClass>, PaginatedList<RoomClassForManagementResponse>>()
               .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
        }
    }
}
