using HttpModels;

namespace HttpApiServer
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<TEntity> GetById(Guid Id);
        Task<TEntity?> FindById(Guid id);

        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(TEntity entity);

        Task<IReadOnlyList<TEntity>> GetAll();

    }
}
