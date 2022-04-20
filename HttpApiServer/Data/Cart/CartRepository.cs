using HttpModels;
using Microsoft.EntityFrameworkCore;

namespace HttpApiServer
{
    public class CartRepository : EfRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Cart> GetCartByAccountId(Guid id)
        {
            return Entities.Where(cart => cart.AccountId == id)
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .FirstAsync();
        }

        public Task<Cart?> FindCartByAccountId(Guid id)
        {
            return Entities.Where(cart => cart.AccountId == id)
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .FirstOrDefaultAsync();
        }
    }
    
}
