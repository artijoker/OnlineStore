using HttpModels;
using Microsoft.EntityFrameworkCore;

namespace HttpApiServer;

public class ProductRepository : EfRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public override Task<Product> GetById(Guid id)
    {
        return Entities.Where(p => p.Id == id).Include(p => p.Category).FirstAsync();
    }

    public override Task<Product?> FindById(Guid id)
    {
        return Entities.Where(p => p.Id == id).Include(p => p.Category).FirstOrDefaultAsync();
    }

    public override async Task<IReadOnlyList<Product>> GetAll()
    {
        return await Entities.Include(p => p.Category).ToListAsync();
    }
}