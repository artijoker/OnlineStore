namespace HttpApiServer
{
    public class UnitOfWorkEf : IUnitOfWork, IAsyncDisposable
    {
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICartItemRepository CartItemRepository { get; }
        public IConfirmationCodeRepository ConfirmationCodeRepositiry { get; }
        public IOrderRepository OrderRepository { get; }



        private readonly AppDbContext _dbContext;

        public UnitOfWorkEf(
            AppDbContext dbContext,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IAccountRepository accountRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IConfirmationCodeRepository confirmationCodeRepositiry,
            IOrderRepository orderRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            ProductRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            CategoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            AccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            CartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            ConfirmationCodeRepositiry = confirmationCodeRepositiry ?? throw new ArgumentNullException(nameof(confirmationCodeRepositiry));
            OrderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            CartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _dbContext.Dispose();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
    }

}
