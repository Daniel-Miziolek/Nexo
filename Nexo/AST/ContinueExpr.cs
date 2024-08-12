using Nexo.Exceptions;
using Nexo.Values;

namespace Nexo.AST
{
    public class ContinueExpr : Expr
    {
        public override Value Eval(Scope scope)
        {
            throw new ContinueException();
        }
    }
}
