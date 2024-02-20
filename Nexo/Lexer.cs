using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
        }

        private char Peek()
        {
            if (_position < _input.Length)
                return _input[_position];
            else
                return '\0';
        }

        private char Next()
        {
            if (_position < _input.Length)
                return _input[_position++];
            else
                return '\0';
        }

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(Peek()))
            {
                Next();
            }
        }

        public Token GetNextToken()
        {
            SkipWhitespace();

            if (_position >= _input.Length)
                return null;

            char currentChar = Peek();

            if (char.IsLetter(currentChar))
            {
                return new Token(Token.Type.Word, ReadWord());
            }
            else if (char.IsDigit(currentChar))
            {
                return new Token(Token.Type.Number, ReadNumber());
            }
            else
            {
                Next();
                return new Token(Token.Type.Symbol, currentChar.ToString());
            }
        }

        private string ReadWord()
        {
            string word = "";

            while (char.IsLetterOrDigit(Peek()))
            {
                word += Next();
            }

            return word;
        }

        private string ReadNumber()
        {
            string number = "";

            while (char.IsDigit(Peek()))
            {
                number += Next();
            }

            return number;
        }
    }
}
