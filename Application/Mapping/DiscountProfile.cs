using Application.DTOs.Discounts;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountResponse>();
            CreateMap<DiscountCreationRequest, Discount>();
            CreateMap<PaginatedList<Discount>, PaginatedList<Discount>>()
             .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
        }
    }
}
