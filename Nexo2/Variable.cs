using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2
{
    public sealed class Variable
    {
        public enum VarType
        {
            Int,
            String,
            List
        }

        public VarType Type { get; }
        public object Value { get; }
        private List<int> listValue;

        public Variable(int value)
        {
            Type = VarType.Int;
            Value = value;
        }

        public Variable(string value)
        {
            Type = VarType.String;
            Value = value;
        }

        public Variable(List<int> value)
        {
            Type = VarType.List;
            listValue = value;
        }

        public int AsInt()
        {
            return (int)Value;
        }

        public string AsString()
        { 
            return (string)Value;
        }


        public List<int> AsList()
        {
            return listValue;
        }
    }
}
