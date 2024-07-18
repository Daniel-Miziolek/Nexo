using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2.Values
{
    public sealed class VoidValue : Value
    {
        public VoidValue()
        {

        }

        public override string ToString()
        {
            return "void";
        }
    }
}
