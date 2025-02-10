using Application.DTOs.Owners;
using Application.DTOs.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping
{
    public class OwnersProfile : Profile
    {
        public OwnersProfile()
        {
            CreateMap<OwnerCreationRequest, Owner>();
            CreateMap<OwnerUpdateRequest, Owner>();
            CreateMap<Owner, OwnersResponse>();
            CreateMap<PaginatedList<Owner>, PaginatedList<OwnersResponse>>();
        }
    }
}
