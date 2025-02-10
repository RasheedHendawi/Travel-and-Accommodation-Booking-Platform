using Application.DTOs.Cities;
using AutoMapper;
using Domain.Entities;
using Domain.Models;


namespace Application.Mapping
{
    public class CitiesProfile : Profile
    {
        public CitiesProfile()
        {
            CreateMap<CityCreationRequest, City>();
            CreateMap<City, TrendingCityResponse>();
            CreateMap<CityUpdateRequest, City>();
            CreateMap<CityForManagement, CityForManagementResponse>();
        }
    }
}
