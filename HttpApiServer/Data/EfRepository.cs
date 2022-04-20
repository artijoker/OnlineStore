using HttpModels;
using Microsoft.EntityFrameworkCore;

namespace HttpApiServer
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity: class, IEntity
    {
        protected readonly AppDbContext _context;
        protected DbSet<TEntity> Entities => _context.Set<TEntity>();

        public EfRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task Add(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public virtual async Task<TEntity?> FindById(Guid id)
        {
            return await Entities.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid Id)
        {
            return await Entities.FirstAsync(it => it.Id == Id);
        }

        public virtual Task Remove(TEntity entity)
        {
            Entities.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
