using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.RoomExceptions
{
    public class DiscountIntervalsException : ExceptionsBase
    {
        public DiscountIntervalsException() : base("Discount intervals are overlapping.")
        {
        }
    }
}
