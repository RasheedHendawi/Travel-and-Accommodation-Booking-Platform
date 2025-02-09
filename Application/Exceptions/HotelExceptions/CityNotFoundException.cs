using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.HotelExceptions
{
    public class CityNotFoundException : ExceptionsBase
    {
        public CityNotFoundException() : base("City not found !")
        {
        }
    }
}
