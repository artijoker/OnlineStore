using HttpModels;

namespace HttpApiServer
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByAccountId(Guid id);
        Task<Cart?> FindCartByAccountId(Guid id);
    }
}
