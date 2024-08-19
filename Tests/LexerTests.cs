using Nexo;

namespace Tests;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void TestKeyWordParsing()
    {
        var tokens = Lexer.Parse("let const if else while fun continue break foo").ToArray();
        var expected = new Token[]
        {
            new Token(TokenType.Variables, "let", null),
            new Token(TokenType.Constant, "const", null),
            new Token(TokenType.If, "if", null),
            new Token(TokenType.Else, "else", null),
            new Token(TokenType.While, "while", null),
            new Token(TokenType.Function, "fun", null),
            new Token(TokenType.Continue, "continue", null),
            new Token(TokenType.Break, "break", null),
            new Token(TokenType.Identifier, "foo", null)
        };

        foreach ((var token, var e) in tokens.Zip(expected))
        {
            Assert.AreEqual(e.Type, token.Type);
            Assert.AreEqual(e.Lexeme, token.Lexeme);
            Assert.AreEqual(e.Literal, token.Literal);
        }
    }
    [TestMethod]
    public void TestOpParsing()
    {
        var tokens = Lexer.Parse("+ - * / % || && = == != > >= < <= ; . , !").ToArray();
        var expected = new Token[]
        {
            new Token(TokenType.Plus, "+", null),
            new Token(TokenType.Minus, "-", null),
            new Token(TokenType.Mul, "*", null),
            new Token(TokenType.Div, "/", null),
            new Token(TokenType.Modulo, "%", null),
            new Token(TokenType.OpOr, "||", null),
            new Token(TokenType.OpAnd, "&&", null),
            new Token(TokenType.Equal, "=", null),
            new Token(TokenType.Comparison, "==", null),
            new Token(TokenType.NotEqual, "!=", null),
            new Token(TokenType.GreaterThan, ">", null),
            new Token(TokenType.GreaterThanOrEqual, ">=", null),
            new Token(TokenType.LessThan, "<", null),
            new Token(TokenType.LessThanOrEqual, "<=", null),
            new Token(TokenType.SemiColon, ";", null),
            new Token(TokenType.Dot, ".", null),
            new Token(TokenType.Comma, ",", null),
            new Token(TokenType.Bang, "!", null),
        };

        foreach ((var token, var e) in tokens.Zip(expected))
        {
            Assert.AreEqual(e.Type, token.Type);
            Assert.AreEqual(e.Lexeme, token.Lexeme);
            Assert.AreEqual(e.Literal, token.Literal);
        }
    }
    [TestMethod]
    public void TestParenParsing()
    {
        var tokens = Lexer.Parse("() {} []").ToArray();
        var expected = new Token[]
        {
            new Token(TokenType.LeftParen, "(", null),
            new Token(TokenType.RightParen, ")", null),
            new Token(TokenType.LeftBrace, "{", null),
            new Token(TokenType.RightBrace, "}", null),
            new Token(TokenType.LeftBracket, "[", null),
            new Token(TokenType.RightBracket, "]", null)
        };

        foreach ((var token, var e) in tokens.Zip(expected))
        {
            Assert.AreEqual(e.Type, token.Type);
            Assert.AreEqual(e.Lexeme, token.Lexeme);
            Assert.AreEqual(e.Literal, token.Literal);
        }
    }
    [TestMethod]
    public void TestLiteralParsing()
    {
        var tokens = Lexer.Parse("""1000 5000 2000.5 0.5 "abc" """).ToArray();
        var expected = new Token[]
        {
            new Token(TokenType.Number, "1000", 1000.0),
            new Token(TokenType.Number, "5000", 5000.0),
            new Token(TokenType.Number, "2000.5", 2000.5),
            new Token(TokenType.Number, "0.5", 0.5),
            new Token(TokenType.String, "abc", "abc"),
        };

        foreach ((var token, var e) in tokens.Zip(expected))
        {
            Assert.AreEqual(e.Type, token.Type);
            Assert.AreEqual(e.Lexeme, token.Lexeme);
            Assert.AreEqual(e.Literal, token.Literal);
        }
    }
}