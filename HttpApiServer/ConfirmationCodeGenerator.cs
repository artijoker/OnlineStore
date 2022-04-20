namespace HttpApiServer
{
    public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
    {
        public string GenerateCode(int length)
        {
            int min = (int)Math.Pow(10, length - 1);
            int max = min * 10 - 1;
            return new Random().Next(min, max).ToString();
        }
    }
}
