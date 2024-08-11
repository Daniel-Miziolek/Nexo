using Nexo.Values;

namespace Nexo.AST
{
    public sealed class BodyExpr(List<Expr> body) : Expr
    {
        private readonly List<Expr> _body = body;

        public override Value Eval(Scope scope)
        {
            var nestedScope = new Scope(scope);
            
            foreach(Expr e in _body)
            {
                e.Eval(nestedScope);
            }

            return new VoidValue();
        }
    }
}
