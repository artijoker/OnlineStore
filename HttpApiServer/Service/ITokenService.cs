using HttpModels;

namespace HttpApiServer
{
    public interface ITokenService
    {
        string GenerateToken(Account account);
    }
}
