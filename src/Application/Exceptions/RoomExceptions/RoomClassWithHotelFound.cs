using Application.Exceptions.ExceptionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.RoomExceptions
{
    public class RoomClassWithHotelFound(string message) : NotFoundExceptions(message)
    {
        public override string Header => "RoomClass found in Hotel";
    }
}
