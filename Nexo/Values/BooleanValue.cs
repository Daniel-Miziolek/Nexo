namespace Nexo.Values
{
    public sealed class BooleanValue(bool value) : Value
    {
        public bool Value { get; init; } = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
