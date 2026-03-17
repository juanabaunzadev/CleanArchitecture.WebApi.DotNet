using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
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
