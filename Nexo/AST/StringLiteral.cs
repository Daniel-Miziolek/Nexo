using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
