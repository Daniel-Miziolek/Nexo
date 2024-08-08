using Nexo.Values;

namespace Nexo.AST
{
    public sealed class BooleanLiteral : Expr
    {
        private bool _value;
        
        public BooleanLiteral(bool value)
        {
            _value = value;
        }

        public override Value Eval(Scope scope)
        {
            return new BooleanValue(_value);
        }
    }
}
