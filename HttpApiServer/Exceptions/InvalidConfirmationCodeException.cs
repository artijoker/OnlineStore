namespace HttpApiServer
{
    public class InvalidConfirmationCodeException : Exception
    {
        public override string? StackTrace { get; }

        public InvalidConfirmationCodeException() : base("Invalid сonfirmation code") { }

        public InvalidConfirmationCodeException(string? message, string? stackTrace = null) : base(message) => StackTrace = stackTrace;
    }
}
