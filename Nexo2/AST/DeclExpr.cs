using Nexo2.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2.AST
{
    public sealed class DeclExpr : Expr
    {
        private readonly string _name;
        private readonly Expr _expr;

        public DeclExpr(string name, Expr expr)
        {
            _name = name;
            _expr = expr;
        }

        public override Value Eval(Scope scope)
        {
            scope.Declare(_name, _expr.Eval(scope));
            return new VoidValue();
        }
    }
}
