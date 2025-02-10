using Application.DTOs.Hotels;
using Application.DTOs.Reviews;
using Application.DTOs.Shared;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class ReviewsProfile : Profile
    {
        public ReviewsProfile()
        {
            CreateMap<ReviewCreationRequest, Review>();
            CreateMap<Review, ReviewResponse>();
            CreateMap<ReviewUpdateRequest, Review>();
            CreateMap<ReviewResponse, Review>();
        }
    }
}
