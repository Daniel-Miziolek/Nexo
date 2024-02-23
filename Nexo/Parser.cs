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
                else
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
                Console.WriteLine("Error: Unexpected token type.");
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

                if (_variables.ContainsKey(nextToken.Value))
                {
                    Console.WriteLine(_variables[nextToken.Value]);
                }
                else
                {
                    while (_position < _tokens.Count)
                    {
                        Console.Write(nextToken.Value + " ");
                        _position++;

                        if (_position < _tokens.Count)
                            nextToken = _tokens[_position];
                    }
                }


                

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error: Incomplete statement.");
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
                                    Console.WriteLine("Error: Division by zero.");
                                    return;
                                }
                            }
                            else if (operatorToken.Value == "^")
                            {
                                result = Math.Pow(firstValue, secondValue);
                            }
                            else
                            {
                                Console.WriteLine("Error: Invalid operator.");
                                return;
                            }

                            Console.WriteLine($"Result: {result}");
                        }
                        else
                        {
                            Console.WriteLine("Error: Invalid number values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Expected operator after the number.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Incomplete statement.");
                }
            }
            else
            {
                Console.WriteLine("Error: Incomplete statement.");
            }
        }

        private void ParseAssignmentOrUseStatement()
        {
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
                                Console.WriteLine($"Error: Invalid number value '{valueToken.Value}'");
                            }
                            _position++;
                        }
                        else
                        {
                            Console.WriteLine("Error: Expected number after '='.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Incomplete statement.");
                    }
                }

            }
            else
            {
                Console.WriteLine("Error: Incomplete statement.");
            }
        }
    }

}