using Nexo.Exceptions;
using Nexo.Values;

namespace Nexo.AST
{
    public sealed class WhileExpr(Expr condition, Expr body) : Expr
    {
        private readonly Expr _condition = condition;
        private readonly Expr _body = body;

        public override Value Eval(Scope scope)
        {
            while (true)
            {
                var condition = _condition.Eval(scope);
                if (condition is BooleanValue b)
                {
                    if (!b.Value)
                    {
                        break;
                    }

                    try
                    {
                        var nestedScope = new Scope(scope);
                        _body.Eval(nestedScope);
                    }
                    catch (BreakException)
                    {
                        break;
                    }

                }
                else
                {
                    throw new UnexpectedTypeException("bool");
                }
            }

            return new VoidValue();
        }
    }
}
