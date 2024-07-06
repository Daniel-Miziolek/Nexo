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
            String
        }

        public VarType Type { get; }
        public object Value { get; }

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

        public int AsInt()
        {
            if (Type != VarType.Int)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
            return (int)Value;
        }

        public string AsString()
        {
            if (Type != VarType.String)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
            return (string)Value;
        }
    }
}
