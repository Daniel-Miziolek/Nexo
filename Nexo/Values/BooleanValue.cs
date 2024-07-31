using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
