using System;
using System.Collections.Generic;


namespace Nexo2
{
    public enum TokenType
    {
        Print,
        If,
        Else,
        Comment,
        Variables,
        Name,
        Value,
        Identifier,
        Number,
        Plus,
        Minus,
        ChValueOfVar,
        Multiply,
        Divide,
        Equal,
        LessThan,
        GreaterThan,
        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        SemiColon
    }

    public interface IExpressionVisitor<T>
    {
        T VisitBinaryExpression(BinaryExpression expr);
        T VisitLiteralExpression(LiteralExpression expr);
    }

    public interface IExpression
    {
        T Accept<T>(IExpressionVisitor<T> visitor);
    }
    
    public class MainF
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                string source = Console.ReadLine();
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                Parser parser = new Parser(tokens);
                parser.Parse();
            }

        }
    }

}
