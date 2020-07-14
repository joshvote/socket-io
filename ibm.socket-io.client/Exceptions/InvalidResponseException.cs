using System;
using System.Collections.Generic;
using System.Text;

namespace IBM.SocketIO.Exceptions
{
    public class InvalidResponseException : Exception
    {
        public InvalidResponseException(string msg)
            : base(msg) { }

        public InvalidResponseException(string msg, Exception inner)
            : base(msg, inner) { }
    }
}
