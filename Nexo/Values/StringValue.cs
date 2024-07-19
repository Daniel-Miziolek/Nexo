using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
