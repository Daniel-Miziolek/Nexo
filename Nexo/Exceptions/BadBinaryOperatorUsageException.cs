using Nexo.AST;

namespace Nexo.Exceptions
{
    public sealed class BadBinaryOperatorUsageException(BinaryExpr.Op op) : NexoException($"Cannot use operator `{op}`.")
    {
        public BinaryOpProxy Op { get; } = new(op);
    }
}
