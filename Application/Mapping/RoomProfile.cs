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
            CreateMap<RoomForManagement, RoomForManagementResponse>();
            CreateMap<Room, RoomForManagementResponse>();
            CreateMap<Room, RoomForGuestResponse>();
            CreateMap<PaginatedList<Room>, PaginatedList<RoomForGuestResponse>>()
                .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<PaginatedList<RoomForManagement>, PaginatedList<RoomForManagementResponse>>()
              .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<PaginatedList<Room>, PaginatedList<RoomForManagementResponse>>()
                    .ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<RoomCreationRequest, CreateRoomHandler>();
            CreateMap<CreateRoomHandler, Room>();
            CreateMap<RoomUpdateRequest, UpdateRoomHandler>();
            CreateMap<UpdateRoomHandler, Room>()
                .ForMember(dst => dst.Number, options => options.MapFrom(src => src.Number));
        }
    }
}
