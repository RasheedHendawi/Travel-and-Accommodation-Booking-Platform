using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.HotelExceptions
{
    public class AmenityNotFoundException : ExceptionsBase
    {
        public AmenityNotFoundException() : base($"Amenity not found.")
        {
        }
    }
}
