using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo
{
    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object Literal { get; }

        public Token(TokenType type, string lexeme, object literal)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
        }
    }
}
