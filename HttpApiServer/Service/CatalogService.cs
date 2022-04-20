
using HttpModels;

namespace HttpApiServer
{
    public class CatalogService
    {
        private readonly IUnitOfWork _unit;
        private readonly IClock _clock;

        public CatalogService(IUnitOfWork unit, IClock clock)
        {
            _unit = unit;
            _clock = clock;
        }

        public async Task AddProduct(Product product)
        {
            var category = await _unit.CategoryRepository.GetById(product.CategoryId);

            product.Id = Guid.NewGuid();
            await _unit.ProductRepository.Add(product);
            category.Products.Add(product);
            await _unit.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProducts(string? userAgent)
        {
            var products = await _unit.ProductRepository.GetAll();
            return SetActualPricesProducts(products.ToList(), userAgent, _clock.GetClock());
        }

        public async Task<IReadOnlyList<Category>> GetCategories()
        {
            return await _unit.CategoryRepository.GetAll();
        }

        public async Task<Product> GetProductById(string? userAgent, Guid id)
        {
            decimal percent = GetPercent(userAgent, _clock.GetClock());
            var products = await _unit.ProductRepository.GetById(id);
            products.Price += (percent == 0 ? 0 : (products.Price / 100) * percent);
            return products;
        }

        public List<Product> SetActualPricesProducts(
            IList<Product> products, 
            string? userAgent = null, 
            DateTime? date = null)
        {
            decimal percent = GetPercent(userAgent, _clock.GetClock());
            return products.Select(p =>
            {
                p.Price += (percent == 0 ? 0 : (p.Price / 100) * percent);
                return p;
            }).ToList();
        }
        private static int GetPercent(string? userAgent = null, DateTime? date = null)
        {
            var dateTime = date ?? DateTime.Now;
            var percent = 0;
            percent -= dateTime.DayOfWeek == DayOfWeek.Wednesday ? 50 : 0;
            percent += dateTime.DayOfWeek == DayOfWeek.Friday ? 50 : 0;
            if (userAgent is not null)
            {
                userAgent = userAgent.ToLower();
                percent -= userAgent.Contains("android") ? 10 : 0;
                percent += userAgent.Contains("iphone") || userAgent.Contains("ipad") ? 50 : 0;
            }

            return percent;
        }
    }
}