using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Abstractions.Security;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken ct = default)
    {
        var passwordHash = _passwordHasher.Hash(command.Password);
        var user = User.Create(command.FirstName, command.LastName, command.Email, passwordHash);
        user.SyncRoles(command.RoleIds);

        try
        {
            var response = await _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync(ct);

            return response.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
