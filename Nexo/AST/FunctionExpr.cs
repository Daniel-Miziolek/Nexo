using Nexo.Values;

namespace Nexo.AST
{
    public sealed class FunctionExpr(string name, string[] args, Expr body) : Expr
    {
        private readonly string _name = name;
        private readonly string[] _args = args;
        private readonly Expr _body = body;

        public override Value Eval(Scope scope)
        {
            scope.Declare(_name, new FunctionValue(_args, _body), true);
            return new VoidValue();
        }
    }
}