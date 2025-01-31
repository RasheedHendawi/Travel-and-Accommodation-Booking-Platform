using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Rooms
{
    public class UpdateRoomHandler
    {
        public Guid RoomClassId { get; init; }
        public Guid RoomId { get; init; }
        public string Number { get; init; }
    }
}
