namespace HttpApiServer;

public class UnidentifiedConfirmationCodeException : Exception
{
    public override string? StackTrace { get; }

    public UnidentifiedConfirmationCodeException() : base("Unidentified confirmation code") { }

    public UnidentifiedConfirmationCodeException(string? message, string? stackTrace = null) : base(message) => StackTrace = stackTrace;
}