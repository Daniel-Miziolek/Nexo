using Nexo.Values;
using System;
using System.Collections.Generic;


namespace Nexo
{
    public enum TokenType
    {
        Print,
        If,
        Else,
        Boolean,
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
        public static void Main()
        {
            Scope scope = new();
            while (true)
            {
                Console.Write("> ");
                string source = Console.ReadLine();
                Lexer lexer = new(source);
                List<Token> tokens = lexer.ScanTokens();

                try
                {
                    var ast = Parser.Parse(tokens);
                    var value = ast.Eval(scope);
                    if (value is not VoidValue)
                    {
                        Console.WriteLine(value);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }                   
            }
        }
    }
}
