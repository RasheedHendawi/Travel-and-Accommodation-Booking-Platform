using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Rooms
{
    public class RoomForManagementResponse
    {
        public Guid Id { get; init; }
        public Guid RoomClassId { get; init; }
        public string Number { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public bool IsAvailable { get; init; }
    }
}
