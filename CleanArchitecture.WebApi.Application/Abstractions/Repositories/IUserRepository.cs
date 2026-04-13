using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
