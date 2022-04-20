namespace HttpApiServer
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IAccountRepository AccountRepository { get; }
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        IConfirmationCodeRepository ConfirmationCodeRepositiry { get; }
        IOrderRepository OrderRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
