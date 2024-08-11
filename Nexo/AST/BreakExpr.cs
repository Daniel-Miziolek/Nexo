using Nexo.Exceptions;
using Nexo.Values;

namespace Nexo.AST
{
    public sealed class BreakExpr : Expr
    {
        public override Value Eval(Scope scope)
        {
            throw new BreakException();
        }
    }
}
