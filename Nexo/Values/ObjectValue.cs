namespace Nexo.Values
{
    public class ObjectValue(Dictionary<string, Value> properties) : Value
    {
        private readonly Dictionary<string, Value> _properties = properties;

        public override string ToString()
        {
            int i = 0;
            string s = "{ ";
            
            foreach (var property in _properties)
            {
                s += property.Key + " = " + property.Value.ToString();
                i++;
                if (i < _properties.Count)
                {
                    s += ", ";
                }
            }

            return s + " }";
        }
    }
}