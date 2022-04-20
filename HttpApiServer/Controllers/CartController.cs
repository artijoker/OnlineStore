using HttpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HttpApiServer
{
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public CartController(CartService cartService, OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("add_to_cart")]
        public async Task<ActionResult<ResponseModel<Product>>> AddToCart(Product product)
        {

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cartService.AddProduct(userId, product.Id);
            return new ResponseModel<Product>() { Succeeded = true };
        }

        [Authorize]
        [HttpGet("get_cart")]
        public async Task<ActionResult<ResponseModel<Cart>>> GetCart()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var products = await _cartService.GetUserCart(userId);


            return new ResponseModel<Cart>()
            {
                Succeeded = true,
                Result = products
            };
        }


        [Authorize]
        [HttpGet("create_order")]
        public async Task<ActionResult<ResponseModel<Order>>> CreateOrder()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _orderService.AddOrder(userId);

            return new ResponseModel<Order>()
            {
                Succeeded = true,
                Result = order
            };
        }
    }
}


