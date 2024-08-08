namespace Nexo.Values
{
    public sealed class StringValue : Value
    {
        public string Value { get; init; }

        public StringValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return '"' + Value + '"';
        }
    }
}
