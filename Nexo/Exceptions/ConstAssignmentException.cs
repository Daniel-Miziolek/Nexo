namespace Nexo.Exceptions
{
    public sealed class ConstAssignmentException(string name) : NexoException($"Cannot assign to `{name}`, beacuse it is const.")
    {
        public string Name { get; } = name;
    }
}
