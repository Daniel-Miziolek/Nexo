using Nexo2.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2.AST
{
    public abstract class Expr
    {
        public abstract Value Eval(Scope scope);
    }
}



