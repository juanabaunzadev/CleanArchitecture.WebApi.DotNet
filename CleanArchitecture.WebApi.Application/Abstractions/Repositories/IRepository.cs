using CleanArchitecture.WebApi.Application.Common;

namespace CleanArchitecture.WebApi.Application.Abstractions.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<PaginatedList<TEntity>> GetPagedAsync(int page, int pageSize);
    Task<TEntity> Add(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
}
