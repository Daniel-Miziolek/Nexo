using Nexo.Values;

namespace Nexo.AST
{
    public abstract class Expr
    {
        public abstract Value Eval(Scope scope);
    }
}



