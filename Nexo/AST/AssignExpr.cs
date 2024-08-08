using Nexo.Values;

namespace Nexo.AST
{
    public sealed class AssignExpr(string left, Expr right) : Expr
    {
        private readonly string _left = left;
        private readonly Expr _right = right;

        public override Value Eval(Scope scope)
        {
            scope.Set(_left, _right.Eval(scope));
            return new VoidValue();
        }
    }
}
