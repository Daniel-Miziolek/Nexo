using Nexo2;
using System.Linq.Expressions;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current = 0;
    public static Dictionary<string, Variable> varaibles = new();
    public static HashSet<string> consts = new();

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
            case TokenType.Constant:
                ParseConstDeclaration();
                break;
            case TokenType.Else:
                break;
            case TokenType.ElseIf:
                break;
            case TokenType.Comment:
                break;
            case TokenType.SemiColon:
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
        Token nextToken = Peek();
        if (nextToken.Type == TokenType.String)
        {
            if (consts.Contains(name))
            {
                PrintError("Cannot assign to constan");
            }
            else
            {
                varaibles[name] = new Variable(nextToken.Lexeme);
                Advance();
            }            
        }
        else
        {
            if (consts.Contains(name))
            {
                PrintError("Cannot assign to constan");
            }
            else
            {
                IExpression expression = ParseExpression();
                varaibles[name] = new Variable((int)expression.Accept(new Interpreter()));
                Console.WriteLine(varaibles[name].Value);
            }
        }

        if (!Consume(TokenType.SemiColon, "Expected ';' after variable declaration."))
        {
            return;
        }
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

        Token valueToken = Peek();
        if (valueToken.Type == TokenType.String)
        {
            varaibles.Add(varName, new Variable(valueToken.Lexeme));
            Console.WriteLine($"Variable '{varName}' has assigned value {valueToken.Lexeme}.");
            Advance();
        }
        else if (valueToken.Type == TokenType.LeftBracket)
        {
            List<int> listValues = ParseList();
            varaibles.Add(varName, new Variable(listValues));
            Console.WriteLine($"Variable '{varName}' has assigned value list value: ");
            foreach( int value in listValues )
            {
                Console.WriteLine(value);
            }
        }
        else
        {
            IExpression expression = ParseExpression();
            varaibles.Add(varName, new Variable((int)expression.Accept(new Interpreter())));
            Console.WriteLine($"Variable '{varName}' has assigned value {(int)expression.Accept(new Interpreter())}.");
        }

        if (!Consume(TokenType.SemiColon, "Expected ';' after variable declaration."))
        {
            return;
        }
    }

    private void ParseConstDeclaration()
    {
        Token varNameToken = Advance();
        if (varNameToken.Type != TokenType.Identifier)
        {
            PrintError("Expected constant name after 'const'.");
            return;
        }
        string varName = varNameToken.Lexeme;

        if (varaibles.ContainsKey(varName))
        {
            PrintError($"Constant '{varName}' is already declared.");
            return;
        }

        if (!Consume(TokenType.Equal, "Expected '=' after const declaration."))
        {
            return;
        }

        Token valueToken = Peek();
        if (valueToken.Type == TokenType.String)
        {
            varaibles.Add(varName, new Variable(valueToken.Lexeme));
            consts.Add(varName);
            Console.WriteLine($"Constant '{varName}' has assigned value {valueToken.Lexeme}.");
            Advance();
        }
        else
        {
            IExpression expression = ParseExpression(); 
            varaibles.Add(varName, new Variable((int)expression.Accept(new Interpreter())));
            consts.Add(varName);
            Console.WriteLine($"Constant '{varName}' has assigned value {(int)expression.Accept(new Interpreter())}.");
        }

        if (!Consume(TokenType.SemiColon, "Expected ';' after const declaration."))
        {
            return;
        }
    }

    private List<int> ParseList()
    {
        List<int> values = new List<int>();
        Advance();

        while (!IsAtEnd() && Peek().Type != TokenType.RightBracket)
        {
            Token valueToken = Advance();
            if (valueToken.Type == TokenType.Number)
            {
                values.Add((int)valueToken.Literal);
            }

            if (Peek().Type == TokenType.Comma)
            {
                Advance();
            }
            else
            {
                break;
            }
        }
        Advance();
        return values;
    }

    private void ParsePrintStatement()
    {
        Token nextToken = Peek();
        if (nextToken.Type == TokenType.String)
        {
            Console.WriteLine(nextToken.Lexeme);
            Advance();
        }
        else if (varaibles.ContainsKey(nextToken.Lexeme))
        {
            Variable var = varaibles[nextToken.Lexeme];
            if (var.Type == Variable.VarType.Int)
                Console.WriteLine(var.AsInt());
            else if (var.Type == Variable.VarType.String)
                Console.WriteLine(var.AsString());
            else if (var.Type == Variable.VarType.List)
            {
                Console.WriteLine(string.Join(",", var.AsList())); 
            }
            Advance();
        }
        else if (nextToken.Type != TokenType.Identifier)
        {
            IExpression expression = ParseExpression();
            Console.WriteLine(expression.Accept(new Interpreter()));
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
            left = new Nexo2.BinaryExpression(left, op, right);
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
