using Application.DTOs.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Models;


namespace Application.Mapping
{
    public class BookingsProfile : Profile
    {
        public BookingsProfile()
        {

            CreateMap<Booking, BookingResponse>()
              .ForMember(dst => dst.HotelName, options => options.MapFrom(src => src.Hotel.Name));
            CreateMap<PaginatedList<Booking>, PaginatedList<BookingResponse>>()
                .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
        }
    }
}
