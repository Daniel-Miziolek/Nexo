using Nexo.AST;
using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nexo.Exceptions
{
    public sealed class BinaryOpProxy(BinaryExpr.Op op)
    {
        private readonly BinaryExpr.Op _op = op;

        public override string ToString()
        {
            return _op switch
            {
                BinaryExpr.Op.Plus => "+",
                BinaryExpr.Op.Minus => "-",
                BinaryExpr.Op.Mul => "*",
                BinaryExpr.Op.Div => "/",
                BinaryExpr.Op.Equal => "=",
                BinaryExpr.Op.LessThan => "<",
                BinaryExpr.Op.GreaterThan => ">",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
