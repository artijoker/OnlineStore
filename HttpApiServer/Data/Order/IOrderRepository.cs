using HttpModels;

namespace HttpApiServer
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task AddOrderItem(OrderItem item);
    }
}
