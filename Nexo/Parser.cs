using Microsoft.VisualBasic;
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
        public static Dictionary<string, int> varaibles = new Dictionary<string, int>();

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
                case TokenType.Variables:
                    ParseVariableDeclaration();
                    break;
                case TokenType.Comment:
                    break;
                case TokenType.ChValueOfVar:
                    ChangeValueOfVariable(currentToken.Lexeme);
                    Advance();
                    break;
                default:
                    PrintError($"Unknow command -> {currentToken.Lexeme}");
                    break;
            }
        }

        private void ChangeValueOfVariable(string name)
        {
            Advance();
            IExpression expression = ParseExpression();
            varaibles[name] = (int)expression.Accept(new Interpreter());
            Console.WriteLine(varaibles[name]);
        }

        private void ParseVariableDeclaration()
        {
            Token varNameToken = Advance();
            if (varNameToken.Type != TokenType.Identifier)
            {
                PrintError("Expected variable name after 'let'.");
                return;
            }
            string varName = varNameToken.Lexeme;

            if (varaibles.ContainsKey(varName))
            {
                PrintError($"Variable '{varName}' is already declared.");
                return;
            }

            if (!Consume(TokenType.Equal, "Expected '=' after variable declaration."))
            {
                return;
            }

            IExpression expression = ParseExpression();
            varaibles.Add(varName, (int)expression.Accept(new Interpreter()));
            Console.WriteLine($"Variable '{varName}' has assigned value {(int)expression.Accept(new Interpreter())}.");

            if (!Consume(TokenType.SemiColon, "Expected ';' after variable declaration."))
            {
                return;
            }
        }

        private void ParsePrintStatement()
        {
            Token nextToken = Peek();
            if (varaibles.ContainsKey(nextToken.Lexeme))
            {
                Console.WriteLine(varaibles[nextToken.Lexeme]);
                Advance();
            }
            else if (nextToken.Type != TokenType.Identifier)
            {
                IExpression expression = ParseExpression();
                Console.WriteLine(expression.Accept(new Interpreter()));
            }
            else
            {
                Console.WriteLine(nextToken.Lexeme);
                Advance();
            }

            if (!Consume(TokenType.SemiColon, "Expected ';' after print statement."))
            {
                return;
            }
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
                else if (Peek().Type == TokenType.ElseIf)
                {
                    IExpression condition2 = ParseExpression();
                    if ((bool)condition2.Accept(new Interpreter()))
                    {
                        Advance();
                        ParseStatement();
                    }
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
                if (!Consume(TokenType.RightParen, "Expected ')' after expression."))
                {
                    return null;
                }
                return expression;
            }
            else
            {
                PrintError($"Unexpected token '{currentToken.Lexeme}'.");
                return null;
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

        private bool Consume(TokenType type, string message)
        {
            if (Peek().Type == type)
            {
                Advance();
                return true;
            }
            else
            {
                PrintError(message);
                return false;
            }
        }

        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
