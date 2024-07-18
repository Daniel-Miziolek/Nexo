using Nexo2.Values;
using System;
using System.Collections.Generic;


namespace Nexo2
{
    public enum TokenType
    {
        Print,
        If,
        Else,
        ElseIf,
        Comment,
        Variables,
        Constant,
        Name,
        Value,
        String,
        Identifier,
        Dot,
        Comma,
        Number,
        Plus,
        Minus,
        ChValueOfVar,
        Mul,
        Div,
        Equal,
        LessThan,
        GreaterThan,
        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        LeftBracket,
        RightBracket,
        SemiColon
    }

  
    public class MainF
    {
        public static void Main(string[] args)
        {
            Scope scope = new();
            while (true)
            {
                Console.Write("> ");
                string source = Console.ReadLine();
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                var ast = Parser.Parse(tokens);
                var value = ast.Eval(scope);
                if (!(value is VoidValue))
                {
                    Console.WriteLine(value);
                }   
            }
        }
    }

}
