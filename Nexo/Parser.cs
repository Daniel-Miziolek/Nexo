using Nexo;
using Nexo.AST;
using System.Linq.Expressions;
using static Nexo.AST.BinaryExpr;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current = 0;

    private Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private Token Current()
    {        
        return _tokens[_current];
    }

    private Token Advance()
    {
        var current = Current();
        _current++;
        return current;
    }

    public static Expr Parse(List<Token> tokens)
    {
        var parser = new Parser(tokens);
        return parser.ParseExpr();
    }

    private Expr ParseExpr()
    {
        return ParseAssignment();
    }

    private Expr ParseAssignment()
    { //x = 5;
        if (Current().Type != TokenType.Identifier || (_tokens.Count <= _current+1 || _tokens[_current+1].Type != TokenType.Equal))
        {
            return ParseVariableDeclaration();
        }

        string name = Advance().Lexeme;

        if (Current().Type != TokenType.Equal)
        {
            throw new Exception("Expected = after name of variable");
        }

        Advance();

        var expr = ParseAdditive();
    
        return new AssignExpr(name, expr);
    }

    private Expr ParseVariableDeclaration()
    {
        if (Current().Type != TokenType.Variables)
        {
            return ParseAdditive();
        }

        Advance();        

        if (Current().Type != TokenType.Identifier)
        {
            Console.WriteLine(Current().Type);
            throw new Exception("Expected name of variable after let");
        }

        string name = Advance().Lexeme;

        if (Advance().Type != TokenType.Equal)
        {
            throw new Exception("Expected = after name of variable");
        }

        var expr = ParseAdditive();

        return new DeclExpr(name, expr);
    }

    private Expr ParseAdditive()
    {
        var left = ParseMultiplicative();

        while (!Eof() && (Current().Type == TokenType.Plus || Current().Type == TokenType.Minus))
        {
            var op = Advance().Type switch
            {
                TokenType.Plus => BinaryExpr.Op.Plus,
                TokenType.Minus => BinaryExpr.Op.Minus,
            };
            var right = ParseMultiplicative();
            left = new BinaryExpr(left, right, op);
        }

        return left;
    }

    private Expr ParseMultiplicative()
    {
        var left = ParsePrimary();

        while (!Eof() && (Current().Type == TokenType.Mul || Current().Type == TokenType.Div))
        {
            var op = Advance().Type switch
            {
                TokenType.Mul => BinaryExpr.Op.Mul,
                TokenType.Div => BinaryExpr.Op.Div,
            };
            var right = ParsePrimary();
            left = new BinaryExpr(left, right, op);
        }

        return left;
    }

    private Expr ParsePrimary()
    {
        switch (Current().Type)
        {
            case TokenType.Number:
                return new NumberLiteral(double.Parse(Advance().Lexeme));
            case TokenType.LeftParen:
                Advance();
                var expr = ParseExpr();
                if (Advance().Type != TokenType.RightParen)
                {
                    throw new Exception("Expected right paren");
                }
                return expr;
            case TokenType.String:
                return new StringLiteral(Advance().Lexeme);
            case TokenType.Print:
                return ParsePrint();
            case TokenType.Identifier:
                return new Identifier(Advance().Lexeme);
            default:
                throw new InvalidOperationException();
        }
    }

    private Expr ParsePrint()
    {
        Advance();
        
        if (Current().Type != TokenType.LeftParen)
        {
            throw new Exception("Expected left paren after print");
        }

        Advance();

        var expr = ParseExpr();

        if (Current().Type != TokenType.RightParen)
        {
            throw new Exception("Expected right paren after expression in print");
        }

        Advance();

        return new PrintExpr(expr);
    }

    private bool Eof()
    {
        return _current >= _tokens.Count;
    }




}
