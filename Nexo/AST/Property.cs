namespace Nexo.AST
{
    public sealed class Property(string name, Expr initializer)
    {
        public string Name { get; } = name;
        public Expr Initializer { get; } = initializer;
    }
}
