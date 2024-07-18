using Nexo2.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2.AST
{
    public sealed class PrintExpr : Expr
    {
        private Expr _expr;

        public PrintExpr(Expr expr)
        {
            _expr = expr;
        }

        public override Value Eval(Scope scope)
        {
            var value = _expr.Eval(scope);
            Console.WriteLine(value);
            return new VoidValue();
        }
    }
}
