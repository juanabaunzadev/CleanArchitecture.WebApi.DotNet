using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Roles.Commands;

[TestClass]
public class CreateRoleCommandHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IRoleRepository _roleRepository;
    private IUnitOfWork _unitOfWork;
    private CreateRoleCommandHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateRoleCommandHandler(_roleRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_Should_CreateRole_When_CommandIsValid()
    {
        // Arrange
        var command = new CreateRoleCommand("Admin");
        var roleCreated = Role.Create(command.Name);
        _roleRepository.Add(Arg.Any<Role>()).Returns(roleCreated);

        // Act
        await _handler.Handle(command);

        // Assert
        await _roleRepository.Received(1).Add(Arg.Any<Role>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_Should_Rollback_When_ExceptionIsThrown()
    {
        // Arrange
        var command = new CreateRoleCommand("Admin");

        _roleRepository.Add(Arg.Any<Role>()).Throws(new Exception("Database error"));

        // Act
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command));

        // Assert
        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
    }
}
