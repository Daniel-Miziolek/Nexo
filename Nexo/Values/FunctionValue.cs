using Nexo.AST;

namespace Nexo.Values
{
    public class FunctionValue : Value
    {
        public string[] Args { get; }
        public Expr Body { get; }        
        public FunctionValue(string[] args, Expr body)
        {
            Args = args;
            Body = body;
        }
    }
}
