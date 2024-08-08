using Nexo.Values;

namespace Nexo.AST
{
    public sealed class Identifier : Expr
    {
        private readonly string _name;
        public Identifier(string name)
        {
            _name = name;
        }

        public override Value Eval(Scope scope)
        {
            return scope.Get(_name);
        }
    }
}
