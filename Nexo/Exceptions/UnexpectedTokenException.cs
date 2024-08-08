namespace Nexo.Exceptions
{
    public class UnexpectedTokenException(Token found, TokenType expected) 
        : InvalidTokenException(found, $"Unexpected token `{found}`, expected `{new TokenTypeProxy(expected)}`.")
    {
        public TokenType Expected { get; } = expected;
    }
}
