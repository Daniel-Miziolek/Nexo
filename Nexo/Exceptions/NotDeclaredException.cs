namespace Nexo.Exceptions
{
    public sealed class NotDeclaredException(string name) : NexoException($"Use of undeclared variable `{name}`.")
    {
        public string Name { get; } = name;
    }
}
