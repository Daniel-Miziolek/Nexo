using Nexo.Values;

namespace Nexo.AST
{
    public class ObjectExpr(Property[] properties) : Expr
    {
        public Property[] _properties = properties;

        public override Value Eval(Scope scope)
        {
            Dictionary<string, Value> obj = [];

            foreach (Property p in _properties)
            {
                obj.Add(p.Name, p.Initializer.Eval(scope));
            }

            return new ObjectValue(obj);
        }
    }
}