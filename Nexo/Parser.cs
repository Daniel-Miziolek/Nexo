using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo2
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public void Parse()
        {
            while (!IsAtEnd())
            {
                ParseStatement();
            }
        }

        private void ParseStatement()
        {
            Token currentToken = Advance();

            switch (currentToken.Type)
            {
                case TokenType.Print:
                    ParsePrintStatement();
                    break;
                case TokenType.If:
                    ParseIfStatement();
                    break;
            }
        }

        private void ParsePrintStatement()
        {
            IExpression expression = ParseExpression();

            Console.WriteLine((int)expression.Accept(new Interpreter()));
        }

        private void ParseIfStatement()
        {
            IExpression condition = ParseExpression();
            if ((bool)condition.Accept(new Interpreter()))
            {
                ParseStatement();
            }
            else
            {
                while (Peek().Type != TokenType.Else && Peek().Type != TokenType.SemiColon && !IsAtEnd())
                {
                    Advance();
                }

                if (Peek().Type == TokenType.Else)
                {
                    Advance(); // Consume 'else'
                    ParseStatement();
                }
            }
        }

        private IExpression ParseExpression()
        {
            return ParseBinaryExpression();
        }

        private IExpression ParseBinaryExpression()
        {
            IExpression left = ParsePrimary();

            while (IsBinaryOperator(Peek().Type))
            {
                Token op = Advance();
                IExpression right = ParsePrimary();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParsePrimary()
        {
            Token currentToken = Advance();
            if (currentToken.Type == TokenType.Number)
            {
                return new LiteralExpression(currentToken.Literal);
            }
            else if (currentToken.Type == TokenType.LeftParen)
            {
                IExpression expression = ParseExpression();
                Consume(TokenType.RightParen, "Expected ')' after expression.");
                return expression;
            }
            else
            {
                throw new Exception($"Unexpected token '{currentToken.Lexeme}'.");
            }
        }

        private Token Advance()
        {
            if (!IsAtEnd())
            {
                _current++;
            }
            return _tokens[_current - 1];
        }

        private Token Peek()
        {
            if (IsAtEnd())
            {
                return _tokens[_tokens.Count - 1];
            }
            return _tokens[_current];
        }

        private bool IsAtEnd()
        {
            return _current >= _tokens.Count;
        }

        private bool IsBinaryOperator(TokenType type)
        {
            return type == TokenType.Plus || type == TokenType.Minus ||
                   type == TokenType.Multiply || type == TokenType.Divide ||
                   type == TokenType.Equal || type == TokenType.LessThan ||
                   type == TokenType.GreaterThan;
        }

        private void Consume(TokenType type, string message)
        {
            if (Peek().Type == type)
            {
                Advance();
            }
            else
            {
                throw new Exception(message);
            }
        }
    }

}
