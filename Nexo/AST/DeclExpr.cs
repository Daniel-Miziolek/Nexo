using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.AST
{
    public sealed class DeclExpr : Expr
    {
        private readonly string _name;
        private readonly Expr _expr;
        private readonly bool _isConstant;

        public DeclExpr(string name, Expr expr, bool isConstant)
        {
            _name = name;
            _expr = expr;
            _isConstant = isConstant;

        }

        public override Value Eval(Scope scope)
        {
            scope.Declare(_name, _expr.Eval(scope), _isConstant);
            return new VoidValue();
        }
    }
}
