namespace HttpApiServer
{
    public class ItemNotFoundCartException : Exception
    {
        public override string? StackTrace { get; }

        public ItemNotFoundCartException() : base("Item not found in cart") { }

        public ItemNotFoundCartException(string? message, string? stackTrace = null) : base(message) => StackTrace = stackTrace;
    }
}
