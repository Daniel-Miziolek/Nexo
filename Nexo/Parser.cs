using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _position;
        private Dictionary<string, int> _variables;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _variables = new Dictionary<string, int>();
        }

        public void Parse()
        {
            while (_position < _tokens.Count)
            {
                ParseStatement();
            }
        }

        private void ParseStatement()
        {
            Token currentToken = _tokens[_position];

            if (currentToken.TokenType == Token.Type.Word)
            {
                if (currentToken.Value == "print")
                {
                    Print();
                }
                else if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "if")
                {
                    Conditions();
                }
                else if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "let")
                {
                    ParseAssignmentOrUseStatement();
                }
            }
            else if (currentToken.TokenType == Token.Type.Number)
            {
                MathematicalOperations();
            }
            else if (currentToken.TokenType == Token.Type.Symbol && currentToken.Value == "#")
            {
                Comment();
            }
            else
            {
                _position++;
            }
        }

        private void ParseAssignmentOrUseStatement()
        {
            _position++;
            Token wordToken = _tokens[_position];
            _position++;

            if (_position < _tokens.Count)
            {
                Token nextToken = _tokens[_position];

                if (nextToken.TokenType == Token.Type.Symbol && nextToken.Value == "=")
                {

                    _position++;

                    if (_position < _tokens.Count)
                    {
                        Token valueToken = _tokens[_position];
                        if (valueToken.TokenType == Token.Type.Number)
                        {
                            int value;
                            if (int.TryParse(valueToken.Value, out value))
                            {
                                _variables[wordToken.Value] = value;
                                Console.WriteLine($"Variable {wordToken.Value} assigned the value {value}");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error: Invalid number value '{valueToken.Value}'");
                                Console.ResetColor();
                            }
                            _position++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Expected number after '='.");
                            Console.ResetColor();
                            _position++;
                        }
                    }
                    
                }

            }
            
        }

        private void Conditions()
        {
            _position++;

            if (_position < _tokens.Count)
            {
                Token n1 = _tokens[_position];
                if (int.TryParse(n1.Value, out int value))
                {
                    _position++;
                    if (_position < _tokens.Count)
                    {
                        Token sym = _tokens[_position];
                        _position++;
                        if (_position < _tokens.Count)
                        {
                            Token n2 = _tokens[_position];
                            if (int.TryParse(n2.Value, out int value2))
                            {
                                if (n1.TokenType == Token.Type.Number && n2.TokenType == Token.Type.Number &&
                                    sym.TokenType == Token.Type.Symbol && sym.Value == ">" && value > value2)
                                {
                                    _position++;
                                    Token klamra = _tokens[_position];
                                    if (klamra.Value == "{")
                                    {
                                        _position++;
                                        if (_position < _tokens.Count)
                                        {
                                            Token currentToken = _tokens[_position];
                                            if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "print")
                                            {
                                                Print();
                                            }
                                            else if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "let")
                                            {
                                                ParseAssignmentOrUseStatement();
                                            }
                                        }
                                    }

                                }
                                else if (n1.TokenType == Token.Type.Number && n2.TokenType == Token.Type.Number &&
                                    sym.TokenType == Token.Type.Symbol && sym.Value == "<" && value < value2)
                                {
                                    _position++;
                                    Token klamra = _tokens[_position];
                                    if (klamra.Value == "{")
                                    {
                                        _position++;
                                        if (_position < _tokens.Count)
                                        {
                                            Token currentToken = _tokens[_position];
                                            if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "print")
                                            {
                                                Print();
                                            }
                                            else if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "let")
                                            {
                                                ParseAssignmentOrUseStatement();
                                            }
                                        }
                                    }
                                }
                                else if (n1.TokenType == Token.Type.Number && n2.TokenType == Token.Type.Number &&
                                    sym.TokenType == Token.Type.Symbol && sym.Value == "=" && value == value2)
                                {
                                    _position++;
                                    Token klamra = _tokens[_position];
                                    if (klamra.Value == "{")
                                    {
                                        _position++;
                                        if (_position < _tokens.Count)
                                        {
                                            Token currentToken = _tokens[_position];
                                            if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "print")
                                            {
                                                Print();
                                            }
                                            else if (currentToken.TokenType == Token.Type.Word && currentToken.Value == "let")
                                            {
                                                ParseAssignmentOrUseStatement();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(false);
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Second token is not a number");
                                Console.ResetColor();
                                _position++;
                            }
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: First token is not a number");
                    Console.ResetColor();
                    _position++;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Not enough tokens to check condition.");
                Console.ResetColor();
                _position++;
            }
        }




        private void Comment()
        {
            while (_position < _tokens.Count && !_tokens[_position].Value.Contains("\n"))
            {
                _position++;
            }
        }


        private void Print()
        {
            _position++;

            if (_position < _tokens.Count)
            {

                Token nextToken = _tokens[_position];
                if (nextToken.TokenType == Token.Type.Symbol && nextToken.Value == "'")
                {
                    _position++;
                    Token nextToken2 = _tokens[_position];
                    if (_position < _tokens.Count)
                    {
                        while (_tokens[_position].Value != "'")
                        {
                            Console.Write(nextToken2.Value + " ");
                            _position++;

                            if (_position < _tokens.Count)
                                nextToken2 = _tokens[_position];
                        }

                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: no ' sign");
                    Console.ResetColor();
                    _position++;
                }


            }

        }






        private void MathematicalOperations()
        {
            Token numberToken = _tokens[_position];
            _position++;

            if (_position < _tokens.Count)
            {
                Token operatorToken = _tokens[_position];
                _position++;

                if (_position < _tokens.Count)
                {
                    Token secondNumberToken = _tokens[_position];
                    _position++;

                    if (operatorToken.TokenType == Token.Type.Symbol)
                    {
                        double firstValue, secondValue;
                        if (Double.TryParse(numberToken.Value, out firstValue) && Double.TryParse(secondNumberToken.Value, out secondValue))
                        {
                            double result = 0;
                            if (operatorToken.Value == "+")
                            {
                                result = firstValue + secondValue;
                            }
                            else if (operatorToken.Value == "-")
                            {
                                result = firstValue - secondValue;
                            }
                            else if (operatorToken.Value == "*")
                            {
                                result = firstValue * secondValue;
                            }
                            else if (operatorToken.Value == "/")
                            {
                                if (secondValue != 0)
                                {
                                    result = firstValue / secondValue;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Error: Division by zero.");
                                    Console.ResetColor();
                                    
                                }
                            }
                            else if (operatorToken.Value == "^")
                            {
                                result = Math.Pow(firstValue, secondValue);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Invalid operator.");
                                Console.ResetColor();
                                
                            }

                            Console.WriteLine($"Result: {result}");
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Expected operator after the number.");
                        Console.ResetColor();
                    }
                }               
            }            
        }

        
    }

}