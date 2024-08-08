namespace Nexo.Exceptions
{
    public class InvalidTokenException(Token found, string message) : NexoException(message)
    {
        public Token Found { get; } = found;
       
        public InvalidTokenException(Token found) : this(found, $"Unexpected token `{found}`.") { }
    }
}
