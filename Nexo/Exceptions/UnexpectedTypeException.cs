using Nexo.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.Exceptions
{
    public sealed class UnexpectedTypeException(string type) : NexoException
    {
        private readonly string _type = type;

        public override string ToString()
        {
            return $"Expected `{_type}`.";
        }
    }
}
