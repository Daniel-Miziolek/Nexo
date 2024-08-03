using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo.Exceptions
{
    public class UnexpectedTokenException(Token found, TokenType expected) : InvalidTokenException(found)
    {
        private readonly TokenTypeProxy _expected = new(expected);

        public override string ToString()
        {
            return $"Unexpected token `{_found}`, expected `{_expected}`.";
        }
    }
}
