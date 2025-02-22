﻿using Application.DTOs.Amenities;
using Application.DTOs.Shared;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class AmenityProfile : Profile
    {
        public AmenityProfile()
        {
            CreateMap<Amenity, AmenityResponse>();
            CreateMap<AmenityCreationRequest, Amenity>();
            CreateMap<AmenityUpdateRequest, Amenity>();
        }
    }
}
