namespace CleanArchitecture.WebApi.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    Task RollbackAsync();
}
