using HttpModels;

namespace HttpApiServer
{
    public class CartItemRepository : EfRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
