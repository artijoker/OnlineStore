using HttpModels;

namespace HttpApiServer
{
    public class ConfirmationCodeRepository : EfRepository<ConfirmationCode>, IConfirmationCodeRepository
    {
        public ConfirmationCodeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
