using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.ExceptionTypes
{
    public class NotFoundExceptions(string message) : ExceptionsBase(message)
    {
        public override string Header => "Contnet not found";
    }
}
