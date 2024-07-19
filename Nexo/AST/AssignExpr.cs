using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.AST
{
    public sealed class AssignExpr : Expr
    {
        private readonly string _left;
        private readonly Expr _right;
        public AssignExpr(string left, Expr right)
        {
            _left = left;
            _right = right;
        }

        public override Value Eval(Scope scope)
        {
            scope.Set(_left, _right.Eval(scope));
            return new VoidValue();
        }
    }
}
