using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo
{
    public class Token
    {
        public enum Type
        {
            Word,
            Number,
            Symbol
        }

        public Type TokenType { get; }
        public string Value { get; }

        public Token(Type tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }


    }
}
