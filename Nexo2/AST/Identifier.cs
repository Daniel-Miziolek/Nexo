using Nexo2.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2.AST
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
