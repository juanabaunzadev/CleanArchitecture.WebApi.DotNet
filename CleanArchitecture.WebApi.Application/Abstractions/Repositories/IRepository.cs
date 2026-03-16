namespace CleanArchitecture.WebApi.Application.Abstractions.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> Add(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
}
