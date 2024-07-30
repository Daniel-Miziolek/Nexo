using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
