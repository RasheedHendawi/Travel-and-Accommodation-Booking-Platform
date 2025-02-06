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

            CreateMap<Hotel, HotelGetResponse>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.City.Country))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Path : null))
                .ForMember(dest => dest.GalleryUrls, opt => opt.MapFrom(src => src.Gallery.Select(img => img.Path)));

            CreateMap<Hotel, HotelGetFromManagment>();
            CreateMap<HotelForManagement, HotelGetFromManagment>();
            CreateMap<Hotel, HotelSearchResponse>();
            CreateMap<RoomClass, HotelFeaturedDealResponse>();
        }
    }
}
