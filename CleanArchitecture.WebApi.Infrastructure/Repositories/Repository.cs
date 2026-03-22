using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.WebApi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<PaginatedList<T>> GetPagedAsync(int page, int pageSize, Specification<T>? spec = null)
    {
        var query = _context.Set<T>().AsQueryable();

        if (spec is not null)
        {
            if (spec.IsNoTracking)
                query = query.AsNoTracking();

            foreach (var include in spec.Includes)
                query = query.Include(include);

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, page, pageSize, totalCount);
    }

    public Task<T> Add(T entity)
    {
        _context.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Update(T entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask;
    }
}
