using Nexo.AST;
using Nexo.Exceptions;
using System.Diagnostics;
using static Nexo.AST.BinaryExpr;

namespace Nexo
{
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
            return ParseKeyWordExpr();
        }

        private Expr ParseKeyWordExpr()
        {
            return Current().Type switch
            {
                TokenType.If => ParseIfExpr(),
                TokenType.Variables | TokenType.Constant => ParseVariableDeclaration(),
                _ => ParseOpExpr()
            };
        }

        private Expr ParseIfExpr()
        {
            if (Current().Type != TokenType.If)
            {
                return ParseOpExpr();
            }

            Advance();

            var condition = ParseOpExpr();

            var body = ParseBody();

            if (!Eof() && Current().Type == TokenType.Else)
            {
                Advance();

                if (Current().Type == TokenType.If)
                {
                    var elseIfBody = ParseIfExpr();
                    return new IfExpr(condition, body, elseIfBody);
                }
                else
                {
                    var elseBody = ParseBody();
                    return new IfExpr(condition, body, elseBody);
                }                
            }

            return new IfExpr(condition, body, null);
        }

        private Expr ParseBody()
        {
            if (Current().Type != TokenType.LeftBrace)
            {
                throw new UnexpectedTokenException(Current(), TokenType.LeftBrace);
            }

            Advance();

            var expr = ParseExpr();

            if (Current().Type != TokenType.RightBrace)
            {
                throw new UnexpectedTokenException(Current(), TokenType.RightBrace);
            }

            Advance();

            return expr;
        }

        private Expr ParseAssignment()
        {
            if (Current().Type != TokenType.Identifier || _tokens.Count <= _current + 1 || _tokens[_current + 1].Type != TokenType.Equal)
            {
                return ParseEqual();
            }

            string name = Advance().Lexeme;

            if (Current().Type != TokenType.Equal)
            {
                throw new UnexpectedTokenException(Current(), TokenType.Equal);
            }

            Advance();

            var expr = ParseEqual();

            return new AssignExpr(name, expr);
        }

        private Expr ParseVariableDeclaration()
        {
            if (Current().Type != TokenType.Variables && Current().Type != TokenType.Constant)
            {
                Console.WriteLine(Current().Type);
                return ParseOpExpr();
            }

            bool isConstant = Advance().Type == TokenType.Constant;

            if (Current().Type != TokenType.Identifier)
            {
                Console.WriteLine(Current().Type);
                throw new UnexpectedTokenException(Current(), TokenType.Identifier);
            }

            string name = Advance().Lexeme;

            if (Current().Type != TokenType.Equal)
            {
                throw new UnexpectedTokenException(Current(), TokenType.Equal);
            }

            Advance();

            var expr = ParseOpExpr();

            return new DeclExpr(name, expr, isConstant);
        }

        private Expr ParseOpExpr()
        {
            return ParseAssignment();
        }

        private Expr ParseEqual()
        {
            var left = ParseGreaterOrLess();

            while (!Eof() && Current().Type == TokenType.Equal)
            {
                var op = Advance().Type switch
                {
                    TokenType.Equal => Op.Equal,
                    _ => throw new UnreachableException(),
                };
                var right = ParseGreaterOrLess();
                left = new BinaryExpr(left, right, op);
            }

            return left;
        }

        private Expr ParseGreaterOrLess()
        {
            var left = ParseAdditive();

            while (!Eof() && (Current().Type == TokenType.GreaterThan || Current().Type == TokenType.LessThan))
            {
                var op = Advance().Type switch
                {
                    TokenType.GreaterThan => Op.GreaterThan,
                    TokenType.LessThan => Op.LessThan,
                    _ => throw new UnreachableException(),
                };
                var right = ParseAdditive();
                left = new BinaryExpr(left, right, op);
            }

            return left;
        }

        private Expr ParseAdditive()
        {
            var left = ParseMultiplicative();

            while (!Eof() && (Current().Type == TokenType.Plus || Current().Type == TokenType.Minus))
            {
                var op = Advance().Type switch
                {
                    TokenType.Plus => Op.Plus,
                    TokenType.Minus => Op.Minus,
                    _ => throw new UnreachableException(),
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
                    TokenType.Mul => Op.Mul,
                    TokenType.Div => Op.Div,
                    _ => throw new UnreachableException(),
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
                    if (Current().Type != TokenType.RightParen)
                    {
                        throw new UnexpectedTokenException(Current(), TokenType.RightParen);
                    }
                    Advance();
                    return expr;
                case TokenType.String:
                    return new StringLiteral(Advance().Lexeme);
                case TokenType.Boolean:
                    return new BooleanLiteral(bool.Parse(Advance().Lexeme));
                case TokenType.Print:
                    return ParsePrint();
                case TokenType.Identifier:
                    return new Identifier(Advance().Lexeme);
                default:
                    throw new InvalidTokenException(Current());
            }
        }

        private PrintExpr ParsePrint()
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
}