using HttpModels;
using Microsoft.EntityFrameworkCore;

namespace HttpApiServer
{
    public class AccountRepository : EfRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }

        public Task<Account?> FindByLogin(string login)
        {
            return Entities.Where(a => a.Login == login).FirstOrDefaultAsync();
        }

        public Task<Account> GetByEmail(string email)
        {
            return Entities.Where(a => a.Email == email).FirstAsync();
        }

        public Task<Account> GetByLogin(string login)
        {
            return Entities.Where(a => a.Login == login).FirstAsync();
        }

        public Task<bool> IsEmailExist(string email)
        {
            return Entities.AnyAsync(a => a.Email == email);
        }

        public Task<bool> IsLoginExist(string login)
        {
            return Entities.AnyAsync(a => a.Login == login);
        }
    }
}
