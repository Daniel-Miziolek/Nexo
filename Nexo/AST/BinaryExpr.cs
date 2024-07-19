using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.AST
{
    public sealed class BinaryExpr : Expr
    {
        private Expr _left;
        private Expr _right;
        private Op _op;

        public enum Op
        {
            Plus,
            Minus,
            Mul,
            Div
        }

        public BinaryExpr(Expr left, Expr right, Op op)
        {
            _left = left;
            _right = right;
            _op = op;   
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
                _ => throw new Exception()
            }; 
        }
    }
}
