using Nexo.Values;

namespace Nexo.AST
{
    public sealed class NumberLiteral : Expr
    {
        private double _value;
        
        public NumberLiteral(double value)
        {
            _value = value;
        }

        public override Value Eval(Scope scope)
        {
            return new Number(_value);
        }
    }
}
