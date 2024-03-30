using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2
{
    public class LiteralExpression : IExpression
    {
        public object Value { get; }

        public LiteralExpression(object value)
        {
            Value = value;
        }

        public T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpression(this);
        }
    }
}
