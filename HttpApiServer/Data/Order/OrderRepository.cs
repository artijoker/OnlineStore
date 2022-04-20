using HttpModels;

namespace HttpApiServer
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddOrderItem(OrderItem item)
        {
           await _context.OrderItems.AddAsync(item);
        }
    }
}
