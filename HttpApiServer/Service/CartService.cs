using HttpModels;

namespace HttpApiServer
{
    public class CartService
    {
        private readonly IUnitOfWork _unit;

        public CartService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task AddProduct(Guid userId, Guid productId, int quantity = 1)
        {
            var cart = await _unit.CartRepository.GetCartByAccountId(userId);
            var cartItem = cart.CartItems.SingleOrDefault(i => i.ProductId == productId);
            if (cartItem is not null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new CartItem() 
                { 
                    Id = Guid.NewGuid(), 
                    CartId = cart.Id, 
                    ProductId = productId, 
                    Quantity = quantity
                };
                await _unit.CartItemRepository.Add(newCartItem);
                cart.CartItems.Add(newCartItem);
            }
            await _unit.CartRepository.Update(cart);
            await _unit.SaveChangesAsync();

        }

        public async Task RenoveProduct(Guid userId, Guid productId)
        {
            var cart = await _unit.CartRepository.GetCartByAccountId(userId);
            var cartItem = cart.CartItems.SingleOrDefault(i => i.ProductId == productId);
            if (cartItem is null)
            {
                throw new ItemNotFoundCartException();
            }
            else
            {
                await _unit.CartItemRepository.Remove(cartItem);
                cart.CartItems.Remove(cartItem);
            }
            await _unit.CartRepository.Update(cart);
            await _unit.SaveChangesAsync();

        }


        public async Task<Cart> GetUserCart(Guid accountId)
        {
            return await _unit.CartRepository.GetCartByAccountId(accountId);
        }

    }
}
