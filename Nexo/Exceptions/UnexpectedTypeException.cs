namespace Nexo.Exceptions
{
    public sealed class UnexpectedTypeException(string type) : NexoException($"Expected `{type}`.")
    {
        public string Type { get; } = type;
    }
}
