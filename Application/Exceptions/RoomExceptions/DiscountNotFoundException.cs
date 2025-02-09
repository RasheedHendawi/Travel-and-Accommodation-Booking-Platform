using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.RoomExceptions
{
    public class DiscountNotFoundException : ExceptionsBase
    {
        public DiscountNotFoundException() : base("Discount Not Found In RoomClass")
        {
        }
    }
}
