using Nexo.Exceptions;
using Nexo.Values;

namespace Nexo.AST
{
    public sealed class BinaryExpr(Expr left, Expr right, BinaryExpr.Op op) : Expr
    {
        private readonly Expr _left = left;
        private readonly Expr _right = right;
        private readonly Op _op = op;        

        public enum Op
        {
            Plus,
            Minus,
            Mul,
            Div,
            Equal,
            GreaterThan,
            LessThan
        }

        public override Value Eval(Scope scope)
        {
            return (_left.Eval(scope), _right.Eval(scope), _op) switch
            {
                (Number left, Number right, Op.Plus) => new Number(left.Value + right.Value),
                (Number left, Number right, Op.Minus) => new Number(left.Value - right.Value),
                (Number left, Number right, Op.Mul) => new Number(left.Value * right.Value),
                (Number left, Number right, Op.Div) => new Number(left.Value / right.Value),
                (StringValue left, StringValue right, Op.Plus) => new StringValue(left.Value + right.Value),
                (Number left, Number right, Op.Equal) => new BooleanValue(left.Value == right.Value),
                (Number left, Number right, Op.GreaterThan) => new BooleanValue(left.Value > right.Value),
                (Number left, Number right, Op.LessThan) => new BooleanValue(left.Value < right.Value),
                (StringValue left, StringValue right, Op.Equal) => new BooleanValue(left.Value == right.Value),
                _ => throw new BadBinaryOperatorUsageException(_op)
            }; 
        }
    }
}
