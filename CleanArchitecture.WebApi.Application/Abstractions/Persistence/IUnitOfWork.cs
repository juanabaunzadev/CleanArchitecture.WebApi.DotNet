namespace CleanArchitecture.WebApi.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
