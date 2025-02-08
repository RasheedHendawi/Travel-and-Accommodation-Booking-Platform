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
            CreateMap<PaginatedList<Discount>, PaginatedList<DiscountResponse>>()
                .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<Discount, DiscountResponse>();
            CreateMap<DiscountCreationRequest, Discount>();

        }
    }
}
