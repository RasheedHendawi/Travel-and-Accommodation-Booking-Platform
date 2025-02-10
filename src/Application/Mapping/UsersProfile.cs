using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<JwtToken, LoginResponse>();
        }
    }
}
