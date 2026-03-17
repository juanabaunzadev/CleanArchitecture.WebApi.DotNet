using CleanArchitecture.WebApi.Application.Abstractions.Persistence;

namespace CleanArchitecture.WebApi.Infrastructure.UnitOfWork;

public class EFCoreUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public EFCoreUnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task RollbackAsync()
    {
        return Task.CompletedTask;
    }
}
