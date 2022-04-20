using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderLibrary
{
    public class NetworkException : Exception
    {
        public override string? StackTrace { get; }

        public NetworkException() : base("Network error") { }

        public NetworkException(string? message, string? stackTrace = null) : base(message) => StackTrace = stackTrace;
    }
}
