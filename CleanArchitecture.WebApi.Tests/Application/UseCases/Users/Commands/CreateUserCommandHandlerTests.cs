using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;
using CleanArchitecture.WebApi.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Users.Commands;

[TestClass]
public class CreateUserCommandHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IUserRepository _userRepository;

    private IValidator<CreateUserCommand> _validator;
    private IUnitOfWork _unitOfWork;
    private CreateUserCommandHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _validator = Substitute.For<IValidator<CreateUserCommand>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateUserCommandHandler(_validator, _userRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_Should_CreateUser_When_CommandIsValid()
    {
        // Arrange
        var command = new CreateUserCommand("John", "Doe", "john.doe@example.com", "Password123!");

        _validator.ValidateAsync(command).Returns(new ValidationResult());
        
        var userCreated = User.Create(command.FirstName, command.LastName, command.Email, command.Password);
        _userRepository.Add(Arg.Any<User>()).Returns(userCreated);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        await _validator.Received(1).ValidateAsync(command);
        await _userRepository.Received(1).Add(Arg.Any<User>());
        await _unitOfWork.Received(1).SaveChangesAsync();
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [TestMethod]
    public async Task Handle_Should_ThrowAppValidationException_When_CommandIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand("", "Doe", "john.doe@example.com", "Password123!");
        var validationFailures = new ValidationResult(new[]
        {
            new ValidationFailure("FirstName", "First name is required.")
        });

        _validator.ValidateAsync(command).Returns(validationFailures);

        // Act
        await Assert.ThrowsAsync<AppValidationException>(() => _handler.Handle(command));

        // Assert
        await _userRepository.DidNotReceive().Add(Arg.Any<User>());
    }

    [TestMethod]
    public async Task Handle_Should_Rollback_When_ExceptionIsThrown()
    {
        // Arrange
        var command = new CreateUserCommand("John", "Doe", "john.doe@example.com", "Password123!");

        _validator.ValidateAsync(command).Returns(new ValidationResult());
        _userRepository.Add(Arg.Any<User>()).Throws(new Exception("Database error"));

        // Act
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command));

        // Assert
        await _unitOfWork.Received(1).RollbackAsync();
    }
}
