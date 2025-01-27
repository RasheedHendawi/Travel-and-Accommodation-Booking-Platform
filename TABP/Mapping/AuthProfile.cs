using Application.Users.Models;
using AutoMapper;
using TABP.DTOs.Users;

namespace TABP.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterRequest, RegisterHandler>();
        }
    }
}
