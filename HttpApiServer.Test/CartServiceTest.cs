using HttpModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HttpApiServer.Test
{
    public class CartServiceTest
    {
        [Fact]
        public async Task New_item_successfully_added_to_cart()
        {
            await using var services = await TestServices.Create();

            var accountId = Guid.NewGuid();
            var cart = new Cart()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId
            };

            await services.Unit.CartRepository.Add(cart);
            await services.Unit.SaveChangesAsync();

            var productId = Guid.NewGuid();

            await services.CartService.AddProduct(accountId, productId);

            Assert.Single(cart.CartItems);
            var item = cart.CartItems.First();
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(1, item.Quantity);
        }

        [Fact]
        public async Task Quantity_of_item_that_have_been_added_to_the_cart_twice_is_2()
        {
            await using var services = await TestServices.Create();

            var accountId = Guid.NewGuid();
            var cart = new Cart()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId
            };

            await services.Unit.CartRepository.Add(cart);
            await services.Unit.SaveChangesAsync();

            var productId = Guid.NewGuid();

            await services.CartService.AddProduct(accountId, productId);
            await services.CartService.AddProduct(accountId, productId);

            Assert.Single(cart.CartItems);
            var item = cart.CartItems.First();
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(2, item.Quantity);
        }

        [Fact]
        public async Task Item_successfully_removed_from_cart()
        {
            await using var services = await TestServices.Create();

            var accountId = Guid.NewGuid();
            var cart = new Cart()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId
            };

            await services.Unit.CartRepository.Add(cart);
            await services.Unit.SaveChangesAsync();

            var productId = Guid.NewGuid();

            await services.CartService.AddProduct(accountId, productId);

            Assert.Single(cart.CartItems);
            var item = cart.CartItems.First();
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(1, item.Quantity);

            await services.CartService.RenoveProduct(accountId, productId);

            Assert.Empty(cart.CartItems);
        }

    }

    public class TestServices : IDisposable, IAsyncDisposable
    {
        public IUnitOfWork Unit { get; }
        public CartService CartService { get; }
        public AppDbContext DbContext { get; }
        private readonly SqliteConnection _connection;

        private TestServices(
            IUnitOfWork unit,
            CartService cartService,
            AppDbContext dbContext,
            SqliteConnection connection)
        {
            Unit = unit;
            CartService = cartService;
            DbContext = dbContext;
            _connection = connection;
        }

        public static async Task<TestServices> Create()
        {
            var (context, connection) = await CreateDbContext();
            IUnitOfWork unit = CreateUnitOfWork(context);
            return new TestServices(unit, new CartService(unit), context, connection);
        }

        private static async Task<(AppDbContext context, SqliteConnection connection)> CreateDbContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:;foreign keys=false");
            await connection.OpenAsync();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return (context, connection);
        }

        private static IUnitOfWork CreateUnitOfWork(AppDbContext context)
        {
            return new UnitOfWorkEf(context,
                new ProductRepository(context),
                new CategoryRepository(context),
                new AccountRepository(context),
                new CartRepository(context),
                new CartItemRepository(context),
                new ConfirmationCodeRepository(context),
                new OrderRepository(context)
                );
        }

        public void Dispose()
        {
            DbContext.Dispose();
            _connection.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}

