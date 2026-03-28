using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Domain.Entities;
using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken ct = default)
    {
        var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

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
