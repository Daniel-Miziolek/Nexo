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
        Number,
        Plus,
        Minus,
        Mul,
        Div,
        Equal,
        Comparison,
        While,
        Break,
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
        private static int lineNumber = 1;

        public static void Main()
        {
            Scope scope = new();
            while (true)
            {
                Console.Write($"{lineNumber}. ");
                string source = Console.ReadLine();                
                lineNumber++;

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
                catch (NexoException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
