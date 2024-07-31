using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.AST
{
    public sealed class IfExpr(Expr condition, Expr body) : Expr
    {
        private readonly Expr _condition = condition;
        private readonly Expr _body = body;

        public override Value Eval(Scope scope)
        {
            var condition = _condition.Eval(scope);
            if (condition is BooleanValue b)
            {
                if (b.Value)
                {
                    var nestedScope = new Scope(scope);
                    return _body.Eval(nestedScope);
                }
                else
                {
                    return new VoidValue();
                }
            }
            throw new Exception("Condition must be bool");
        }
    }
}
