using EmailSenderLibrary;
using HttpModels;
using System.Text;

namespace HttpApiServer
{
    public class OrderService
    {
        private readonly IUnitOfWork _unit;
        private readonly IEmailSender _emailSender;

        public OrderService(IUnitOfWork unit, IEmailSender emailSender)
        {
            _unit = unit;
            _emailSender = emailSender;
        }

        public async Task<Order> AddOrder(Guid accountId)
        {
            var account = await _unit.AccountRepository.GetById(accountId);
            var cart = await _unit.CartRepository.GetCartByAccountId(accountId);
            if (cart.CartItems.Count == 0)
                throw new EmptyСartException();

            Order order = new()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
            };
            StringBuilder builder = new StringBuilder();
            decimal sum = 0;

            builder.Append("Список товаров:\n");
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Order = order,
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity
                };
                builder.Append($"Товар {item.Product!.Name}, в количестве {item.Quantity} шт. Цена за одну единицу {item.Product.Price} p.\n");
                sum += item.Product.Price * item.Quantity;
                await _unit.OrderRepository.AddOrderItem(orderItem);
            }
            await _unit.OrderRepository.Add(order);

            builder.Append($"Общая сумма заказа {sum} p.");
            cart.CartItems.Clear();
            await _emailSender.SendMessage(account.Email, "Закакз из интрнет магазина", builder.ToString());

            await _unit.SaveChangesAsync();

            return order;
        }
    }
}
