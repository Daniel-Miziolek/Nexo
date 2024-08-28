using Nexo.AST;
using Nexo.Exceptions;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
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
            if (current.Type != TokenType.EoF)
            {
                _current++;
            }
            return current;
        }

        public static Expr Parse(List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.EoF, "EoF", null));
            var parser = new Parser(tokens);
            return parser.ParseTopLevel();
        }

        private BodyExpr ParseTopLevel()
        {
            List<Expr> exprs = [];
                
            while (!Eof())
            {
                var expr = ParseExpr();
                exprs.Add(expr);
                
                if (Current().Type == TokenType.SemiColon)
                {
                    Advance();
                }
                else if (expr is not IfExpr)
                {
                    break;
                }
                else if (expr is not WhileExpr)
                {
                    break;
                }
            }

            if (!Eof())
            {
                throw new UnexpectedTokenException(Current(), TokenType.SemiColon);
            }

            return new BodyExpr(exprs);
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
                TokenType.Variables or TokenType.Constant => ParseVariableDeclaration(),
                TokenType.While => ParseWhileExpr(),
                TokenType.Function => ParseFunctionDeclaration(),
                _ => ParseOpExpr()
            };
        }       

        private Property ParseProperty()
        {
            if (Current().Type != TokenType.Identifier)
            {
                throw new UnexpectedTokenException(Current(), TokenType.Identifier);
            }

            string name = Advance().Lexeme;

            if (Current().Type != TokenType.Equal)
            {
                throw new UnexpectedTokenException(Current(), TokenType.Equal);
            }

            Advance();
            
            Expr initializer = ParseOpExpr();
            

            return new Property(name, initializer);
        }

        private Expr ParseFunctionDeclaration()
        {
            if (Current().Type != TokenType.Function)
            {
                return ParseOpExpr();
            }

            Advance();

            string functionName = Advance().Lexeme;

            if (Current().Type != TokenType.LeftParen)
            {
                throw new UnexpectedTokenException(Current(), TokenType.LeftParen);
            }

            Advance();

            List<string> args = [];

            if (Current().Type != TokenType.RightParen)
            {
                do
                {
                    if (Current().Type != TokenType.Identifier)
                    {
                        throw new UnexpectedTokenException(Current(), TokenType.Identifier);
                    }

                    args.Add(Advance().Lexeme);

                    if (Current().Type == TokenType.Comma)
                    {
                        Advance();
                    }
                    else
                    {
                        break;
                    }
                } while (!Eof());
            }

            if (Current().Type != TokenType.RightParen)
            {
                throw new UnexpectedTokenException(Current(), TokenType.RightParen);
            }                        

            Advance();

            var body = ParseBody();

            return new FunctionExpr(functionName, args.ToArray(), body);
        }

        private Expr ParseContinueExpr()
        {
            if (Current().Type != TokenType.Continue)
            {
                return ParseOpExpr();
            }

            Advance();

            return new ContinueExpr();
        }

        private Expr ParseBreakExpr()
        {
            if (Current().Type != TokenType.Break)
            {
                return ParseOpExpr();
            }

            Advance();

            return new BreakExpr();
        }

        private Expr ParseWhileExpr()
        {
            if (Current().Type != TokenType.While)
            {
                return ParseOpExpr();
            }

            Advance();

            var condition = ParseOpExpr();

            var body = ParseBody();

            return new WhileExpr(condition, body);
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

            List<Expr> body = [];

            while (Current().Type != TokenType.RightBrace)
            {
                body.Add(ParseExpr());
                if (Current().Type != TokenType.SemiColon)
                {
                    break;
                }
                else
                {
                    Advance();
                }
            }

            if (Current().Type != TokenType.RightBrace)
            {
                throw new UnexpectedTokenException(Current(), TokenType.RightBrace);
            }

            Advance();

            return new BodyExpr(body);
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

        private Expr ParseAssignment()
        {
            if (Current().Type != TokenType.Identifier || _tokens.Count <= _current + 1 || _tokens[_current + 1].Type != TokenType.Equal)
            {
                return ParseOrExpr();
            }

            string name = Advance().Lexeme;

            if (Current().Type != TokenType.Equal)
            {
                throw new UnexpectedTokenException(Current(), TokenType.Equal);
            }

            Advance();

            var expr = ParseOrExpr();

            return new AssignExpr(name, expr);
        }

        private Expr ParseOrExpr()
        {
            var left = ParseAndExpr();

            while (!Eof() && Current().Type == TokenType.OpOr)
            {
                Advance();

                var right = ParseAndExpr();
                left = new BinaryExpr(left, right, BinaryExpr.Op.Or);
            }

            return left;
        }

        private Expr ParseAndExpr()
        {
            var left = ParseEqual();

            while (!Eof() && Current().Type == TokenType.OpAnd)
            {
                Advance();
                
                var right = ParseEqual();
                left = new BinaryExpr(left, right, BinaryExpr.Op.And);
            }

            return left;
        }

        private Expr ParseEqual()
        {
            var left = ParseGreaterOrLess();

            while (!Eof() && Current().Type == TokenType.Comparison || Current().Type == TokenType.NotEqual)
            {
                var op = Advance().Type switch
                {
                    TokenType.Comparison => Op.Comparsion,
                    TokenType.NotEqual => Op.NotEqual,
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

            while (!Eof() && (Current().Type == TokenType.GreaterThan || Current().Type == TokenType.LessThan || Current().Type == TokenType.GreaterThanOrEqual || Current().Type == TokenType.LessThanOrEqual))
            {
                var op = Advance().Type switch
                {
                    TokenType.GreaterThan => Op.GreaterThan,
                    TokenType.LessThan => Op.LessThan,
                    TokenType.GreaterThanOrEqual => Op.GreaterThanOrEqual,
                    TokenType.LessThanOrEqual => Op.LessThanOrEqual,
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
            var left = ParseModulo();

            while (!Eof() && (Current().Type == TokenType.Mul || Current().Type == TokenType.Div))
            {
                var op = Advance().Type switch
                {
                    TokenType.Mul => Op.Mul,
                    TokenType.Div => Op.Div,
                    _ => throw new UnreachableException(),
                };
                var right = ParseModulo();
                left = new BinaryExpr(left, right, op);
            }

            return left;
        }

        private Expr ParseModulo()
        {
            var left = ParseFunctionCall();

            while (!Eof() && (Current().Type == TokenType.Modulo))
            {
                var op = Advance().Type switch
                {
                    TokenType.Modulo => Op.Modulo,
                    _ => throw new UnreachableException(),
                };
                var right = ParseFunctionCall();
                left = new BinaryExpr(left, right, op);
            }

            return left;
        }

        private Expr ParseFunctionCall()
        {
            var call = ParsePrimary();
            List<Expr> args = [];

            if (Current().Type != TokenType.LeftParen)
            {
                return call;
            }

            Advance();

            if (Current().Type != TokenType.RightParen)
            {
                do
                {
                    args.Add(ParseExpr());

                    if (Current().Type == TokenType.Comma)
                    {
                        Advance();
                    }
                    else
                    {
                        break;
                    }
                } while (!Eof());
            }

            if (Current().Type != TokenType.RightParen)
            {
                throw new UnexpectedTokenException(Current(), TokenType.RightParen);
            }

            Advance();

            return new CallExpr(call, [.. args]);
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

        private Expr ParsePrimary()
        {
            switch (Current().Type)
            {
                case TokenType.Number:
                    return new NumberLiteral(double.Parse(Advance().Lexeme, CultureInfo.InvariantCulture));
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
                case TokenType.Break:
                    return ParseBreakExpr();
                case TokenType.Continue:
                    return ParseContinueExpr();
                case TokenType.Identifier:
                    return new Identifier(Advance().Lexeme);
                case TokenType.LeftBrace:
                    Advance();

                    List<Property> properties = [];

                    while (Current().Type != TokenType.RightBrace)
                    {
                        properties.Add(ParseProperty());

                        if (Current().Type != TokenType.Comma)
                        {
                            break;
                        }

                        Advance();
                    }
                    
                    if (Current().Type != TokenType.RightBrace)
                    {
                        throw new UnexpectedTokenException(Current(), TokenType.RightBrace);
                    }

                    Advance();

                    return new ObjectExpr([.. properties]);
                default:
                    throw new InvalidTokenException(Current());
            }
        }

        private bool Eof()
        {
            return Current().Type == TokenType.EoF;
        }
    }
}