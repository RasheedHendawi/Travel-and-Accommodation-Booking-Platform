using Application.Users.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<RegisterHandler, User>();
            CreateMap<JwtToken, LoginResult>();
        }
    }
}
