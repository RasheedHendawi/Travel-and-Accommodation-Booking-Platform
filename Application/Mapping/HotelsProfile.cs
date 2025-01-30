using Application.DTOs.Hotels;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping
{
    public class HotelsProfile : Profile
    {
        public HotelsProfile()
        {
            CreateMap<HotelCreationRequest, Hotel>();
            CreateMap<HotelUpdateRequest, Hotel>();
            CreateMap<Hotel, HotelGetFromManagment>();
            CreateMap<Hotel, HotelGetResponse>();
            CreateMap<HotelForManagement, HotelGetFromManagment>();
            CreateMap<Hotel, HotelSearchResponse>();
            CreateMap<RoomClass, HotelFeaturedDealResponse>();
        }
    }
}
