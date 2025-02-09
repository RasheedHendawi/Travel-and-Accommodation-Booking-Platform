using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.RoomExceptions
{
    public class RoomClassWithHotelFound : ExceptionsBase
    {
        public RoomClassWithHotelFound() : base($"RoomClass found in Hotel with")
        {
        }
    }
}
