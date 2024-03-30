using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2
{
    public class Interpreter : IExpressionVisitor<object>
    {
        public object Interpret(IExpression expression)
        {
            return expression.Accept(this);
        }

        public object VisitBinaryExpression(BinaryExpression expr)
        {
            object left = Interpret(expr.Left);
            object right = Interpret(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Plus:
                    return (int)left + (int)right;
                case TokenType.Minus:
                    return (int)left - (int)right;
                case TokenType.Multiply:
                    return (int)left * (int)right;
                case TokenType.Divide:
                    return (int)left / (int)right;
                case TokenType.Equal:
                    return (int)left == (int)right;
                case TokenType.LessThan:
                    return (int)left < (int)right;
                case TokenType.GreaterThan:
                    return (int)left > (int)right;
                default:
                    throw new Exception($"Invalid operator '{expr.Operator.Lexeme}'.");
            }
        }

        public object VisitLiteralExpression(LiteralExpression expr)
        {
            return expr.Value;
        }
    }

}
