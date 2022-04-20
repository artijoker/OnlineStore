using Microsoft.EntityFrameworkCore;
using HttpModels;

namespace HttpApiServer;

public class CategoryRepository : EfRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }
}