using Nexo.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.Exceptions
{
    public sealed class ConstAssignmentException(string name) : NexoException
    {
        private readonly string _name = name;

        public override string ToString()
        {
            return $"Cannot assign to `{_name}`, beacuse it is const.";
        }
    }
}
