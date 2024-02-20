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
                ParseAssignmentOrUseStatement();
            }
            else
            {
               
                Console.WriteLine("Error: Unexpected token type.");
                _position++; 
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
