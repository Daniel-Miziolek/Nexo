using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2
{
    public class Lexer
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        public Lexer(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken(); 
            }

         
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LeftParen);
                    break;
                case ')':
                    AddToken(TokenType.RightParen);
                    break;
                case '{':
                    AddToken(TokenType.LeftBrace);
                    break;
                case '}':
                    AddToken(TokenType.RightBrace);
                    break;
                case '+':
                    AddToken(TokenType.Plus);
                    break;
                case '-':
                    AddToken(TokenType.Minus);
                    break;
                case '*':
                    AddToken(TokenType.Multiply);
                    break;
                case '/':
                    AddToken(TokenType.Divide);
                    break;
                case '=':
                    AddToken(TokenType.Equal);
                    break;
                case '<':
                    AddToken(TokenType.LessThan);
                    break;
                case '>':
                    AddToken(TokenType.GreaterThan);
                    break;
                case ';':
                    AddToken(TokenType.SemiColon);
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    _line++;
                    break;
                case '#':
                    while (Peek() != '\n' && !IsAtEnd())
                    {
                        Advance();
                    }
                    break;
                default:
                    if (char.IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (char.IsLetter(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        throw new Exception($"Unexpected character '{c}' at line {_line}.");
                    }
                    break;
            }
        }

        private void ScanNumber()
        {
            while (char.IsDigit(Peek()))
            {
                Advance();
            }

            string number = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(TokenType.Number, number, int.Parse(number)));
        }

        private void ScanIdentifier()
        {
            while (char.IsLetterOrDigit(Peek()))
            {
                Advance();
            }

            string text = _source.Substring(_start, _current - _start);
            TokenType type = TokenType.Identifier;
            if (text == "print") type = TokenType.Print;
            else if (text == "if") type = TokenType.If;
            else if (text == "else") type = TokenType.Else;
            else if (text == "let") type = TokenType.Variables;
            else if (text == "#") type = TokenType.Comment;

            _tokens.Add(new Token(type, text, null));
        }

        private char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private void AddToken(TokenType type)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, null));
        }
    }
}
