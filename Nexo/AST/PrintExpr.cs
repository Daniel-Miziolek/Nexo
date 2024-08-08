using Nexo.Values;

namespace Nexo.AST
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
