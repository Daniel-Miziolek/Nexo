using System;
using System.Collections.Generic;
using System.IO;
using Nexo.Exceptions;
using Nexo.Values;

namespace Nexo
{
    public enum TokenType
    {
        Print,
        If,
        Else,
        Boolean,
        Variables,
        Constant,
        String,
        Identifier,
        Dot,
        Comma,
        Hash,
        Number,
        Plus,
        Minus,
        Mul,
        Div,
        Modulo,
        Equal,
        Comparison,
        NotEqual,
        Bang,
        While,
        Break,
        Continue,
        And,
        OpAnd,
        Or,
        OpOr,
        Function,
        Class,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        LeftBracket,
        RightBracket,
        SemiColon,
        EoF
    }

    public class MainF
    {
        public static void Main()
        {
            Scope scope = new();

            string filePath = @"C:\Users\Daniel\Desktop\nexoCode.txt";

            try
            {
                string source = File.ReadAllText(filePath);

                List<Token> tokens = Lexer.Parse(source);

                try
                {
                    var ast = Parser.Parse(tokens);
                    var value = ast.Eval(scope);                    
                }
                catch (NexoException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
    }
}
