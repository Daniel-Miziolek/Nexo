using System;
using System.Collections.Generic;

namespace Nexo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">");

                string input = Console.ReadLine();

                Lexer lexer = new Lexer(input);
                List<Token> tokens = new List<Token>();

                Token token;
                while ((token = lexer.GetNextToken()) != null)
                {
                    tokens.Add(token);
                }



                Parser parser = new Parser(tokens);
                parser.Parse();

                if (input == "break")
                {
                    Console.WriteLine("The program has ended");
                    break;
                }
            }
            
        }
    }

}
