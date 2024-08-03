using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.Exceptions
{
    public class InvalidTokenException(Token found) : NexoException
    {
        protected readonly Token _found = found;

        public override string ToString()
        {
            return $"Unexpected token `{_found}`.";
        }
    }
}
