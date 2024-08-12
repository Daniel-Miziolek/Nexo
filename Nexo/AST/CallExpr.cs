using Nexo.Exceptions;
using Nexo.Values;

namespace Nexo.AST
{
    public class CallExpr(Expr call, Expr[] args) : Expr
    {
        private readonly Expr _call = call;
        private readonly Expr[] _args = args;

        public override Value Eval(Scope scope)
        {
            var function = _call.Eval(scope) as FunctionValue ?? throw new UnexpectedTypeException("function");
            var functionScope = new Scope(); 

            foreach (var arg in _args.Zip(function.Args))
            {
                functionScope.Declare(arg.Second, arg.First.Eval(scope), false);
            }

            return function.Body.Eval(functionScope);
        }
    }
}
