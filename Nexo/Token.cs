using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo
{
    public class Token(TokenType type, string lexeme, object? literal)
    {
        public TokenType Type { get; } = type;
        public string Lexeme { get; } = lexeme;
        public object? Literal { get; } = literal;

        public override string ToString() 
        {
            return Lexeme; 
        }
    }
}
