namespace HttpApiServer
{
    public class EmptyСartException : Exception
    {
        public override string? StackTrace { get; }

        public EmptyСartException() : base("Empty cart") { }

        public EmptyСartException(string? message, string? stackTrace = null) : base(message) => StackTrace = stackTrace;
    }
}
