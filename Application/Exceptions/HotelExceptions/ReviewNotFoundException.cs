using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.HotelExceptions
{
    public class ReviewNotFoundException : ExceptionsBase
    {
        public ReviewNotFoundException() : base("Review not found.")
        {
        }
    }
}
