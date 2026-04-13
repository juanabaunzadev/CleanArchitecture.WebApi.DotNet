using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.UpdateUser;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Users.Commands;

[TestClass]
public class UpdateUserCommandHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IUserRepository _userRepository;
    private IUnitOfWork _unitOfWork;
    private UpdateUserCommandHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateUserCommandHandler(_userRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_WhenUserExists_ShouldUpdateUser()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "Test",
            "User",
            "test@example.com",
            []
        );

        var user = User.Create("Test", "User", "test@example.com", "hashedPassword");

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        await _handler.Handle(command);

        // Assert
        await _userRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "Test",
            "User",
            "test@example.com",
            []
        );

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command));
    }

    [TestMethod]
    public async Task Handle_WhenExceptionOccurs_ShouldRollback()
    {
        // Arrange
        var user = User.Create("Test", "User", "test@example.com", "hashedPassword");
        var id = user.Id;
        var command = new UpdateUserCommand(
            id,
            "Test",
            "User",
            "test@example.com",
            []
        );

        _userRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(user);
        _userRepository.Update(user).Throws(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command));
        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
    }
}
