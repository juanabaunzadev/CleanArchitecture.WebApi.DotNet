using System.Linq.Expressions;

namespace CleanArchitecture.WebApi.Application.Common;

public abstract class Specification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    public bool IsNoTracking { get; private set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = Criteria is null ? criteria : Criteria.AndAlso(criteria);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;

    protected void AddOrderByDescending(Expression<Func<T, object>> orderBy) => OrderByDescending = orderBy;

    protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);

    protected void AsNoTracking() => IsNoTracking = true;
}
