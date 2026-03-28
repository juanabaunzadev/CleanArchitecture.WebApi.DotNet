using CleanArchitecture.WebApi.Application.Abstractions.Persistence;

namespace CleanArchitecture.WebApi.Infrastructure.UnitOfWork;

public class EFCoreUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public EFCoreUnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

    public Task RollbackAsync(CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}
