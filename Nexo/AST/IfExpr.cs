using Nexo.Exceptions;
using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.AST
{
    public sealed class IfExpr(Expr condition, Expr body, Expr? elseBody) : Expr
    {
        private readonly Expr _condition = condition;
        private readonly Expr _body = body;
        private readonly Expr? _elseBody = elseBody;

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

                if (_elseBody is not null)
                {
                    var nestedScope = new Scope(scope);
                    return _elseBody.Eval(nestedScope);
                }

                return new VoidValue();                
            }
            throw new UnexpectedTypeException("bool");
        }
    }
}
