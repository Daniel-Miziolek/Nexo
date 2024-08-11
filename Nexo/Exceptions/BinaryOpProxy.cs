using Nexo.AST;

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
                BinaryExpr.Op.Comparsion => "==",
                BinaryExpr.Op.LessThan => "<",
                BinaryExpr.Op.GreaterThan => ">",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
