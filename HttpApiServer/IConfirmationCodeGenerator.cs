namespace HttpApiServer
{
    public interface IConfirmationCodeGenerator
    {
        string GenerateCode(int length);
    }
}
