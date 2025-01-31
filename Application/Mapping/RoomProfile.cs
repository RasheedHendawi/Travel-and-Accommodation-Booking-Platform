using Application.DTOs.Rooms;
using AutoMapper;
using Domain.Entities;
using Domain.Models;


namespace Application.Mapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<PaginatedList<Room>, PaginatedList<RoomForGuestResponse>>()
                .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<PaginatedList<RoomForManagement>, PaginatedList<RoomForManagementResponse>>()
              .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<RoomCreationRequest, CreateRoomHandler>();
            CreateMap<CreateRoomHandler, Room>();
            CreateMap<RoomUpdateRequest, UpdateRoomHandler>();
            CreateMap<UpdateRoomHandler, Room>();
        }
    }
}
