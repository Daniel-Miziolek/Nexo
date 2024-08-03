using Nexo.AST;
using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nexo.Exceptions
{
    public sealed class TokenTypeProxy(TokenType tokenType)
    {
        private readonly TokenType _tokenType = tokenType;

        public override string ToString()
        {
            return _tokenType switch
            {
                TokenType.Print => "print",
                TokenType.If => "if",
                TokenType.Else => "else",
                TokenType.ElseIf => "elseif",
                TokenType.Variables => "variables",
                TokenType.Constant => "constant",
                TokenType.Identifier => "identifier",
                TokenType.String => "string",
                TokenType.Number => "number",
                TokenType.Boolean => "boolean",
                TokenType.Dot => ".",
                TokenType.SemiColon => ";",
                TokenType.Comma => ",",
                TokenType.Plus => "+",
                TokenType.Minus => "-",
                TokenType.Mul => "*",
                TokenType.Div => "/",
                TokenType.Equal => "=",
                TokenType.LessThan => "<",
                TokenType.GreaterThan => ">",
                TokenType.LeftParen => "(",
                TokenType.RightParen => ")",
                TokenType.LeftBrace => "{",
                TokenType.RightBrace => "}",
                TokenType.LeftBracket => "[",
                TokenType.RightBracket => "]",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
