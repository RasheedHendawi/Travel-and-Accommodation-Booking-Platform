using Application.DTOs.Amenities;
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
            CreateMap<RoomClass, RoomClassForManagementResponse>()
              .ForMember(dst => dst.RoomClassId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dst => dst.ActiveDiscount, options => options.MapFrom(src => src.Discounts.FirstOrDefault()));
            CreateMap<PaginatedList<RoomClass>, PaginatedList<RoomClassForManagementResponse>>()
               .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<Amenity, AmenityResponse>();
        }
    }
}
