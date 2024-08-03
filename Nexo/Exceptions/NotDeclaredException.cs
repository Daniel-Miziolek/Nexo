using Nexo.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.Exceptions
{
    public sealed class NotDeclaredException(string name) : NexoException
    {
        private readonly string _name = name;

        public override string ToString()
        {
            return $"Use of undeclared variable `{_name}`.";
        }
    }
}
