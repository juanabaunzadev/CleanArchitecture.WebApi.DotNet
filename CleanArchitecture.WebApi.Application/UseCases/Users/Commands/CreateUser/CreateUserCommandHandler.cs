using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Domain.Entities;
using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler
{
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IValidator<CreateUserCommand> validator,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateUserCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            throw new AppValidationException(validationResult);
        }

        var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);
        try
        {
            var response = await _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return response.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
