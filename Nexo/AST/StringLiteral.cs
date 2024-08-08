using Nexo.Values;

namespace Nexo.AST
{
    public sealed class StringLiteral : Expr
    {
        private string _value;

        public StringLiteral(string value)
        {
            _value = value;
        }

        public override Value Eval(Scope scope)
        {
            return new StringValue(_value);
        }
    }
}
