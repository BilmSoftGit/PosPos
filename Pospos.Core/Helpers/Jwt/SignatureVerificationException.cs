using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Core.Helpers.Jwt
{
    public class SignatureVerificationException : Exception
    {
        public SignatureVerificationException(string message):base(message)
        {

        }
    }

    public class TokenExpiredException : Exception
    {
        public TokenExpiredException(string message) : base(message)
        {

        }
    }
}
