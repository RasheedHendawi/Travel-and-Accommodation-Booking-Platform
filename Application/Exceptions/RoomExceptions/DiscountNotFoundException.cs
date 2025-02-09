using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.RoomExceptions
{
    public class DiscountNotFoundException(string message) : ExceptionsBase(message)
    {
        public override string Header => "Discount not found";
    }
}
