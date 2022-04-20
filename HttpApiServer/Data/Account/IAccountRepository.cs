using HttpModels;

namespace HttpApiServer
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByEmail(string email);
        Task<Account> GetByLogin(string login);
        Task<Account?> FindByLogin(string login);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsLoginExist(string login);
    }
}
