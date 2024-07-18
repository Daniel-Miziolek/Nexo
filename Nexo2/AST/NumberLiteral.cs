using Nexo2.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2.AST
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
