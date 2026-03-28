using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Common;

namespace CleanArchitecture.WebApi.Application.Abstractions.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<PaginatedList<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null, CancellationToken ct = default);
    Task<TEntity> Add(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
}
