using Nexo.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.Exceptions
{
    public sealed class BadBinaryOperatorUsageException(BinaryExpr.Op op) : NexoException
    {
        private readonly BinaryOpProxy _op = new(op);

        public override string ToString()
        {
            return $"Cannot use operator `{_op}`.";
        }
    }
}
