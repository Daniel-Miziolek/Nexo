using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.AST
{
    public abstract class Expr
    {
        public abstract Value Eval(Scope scope);
    }
}



