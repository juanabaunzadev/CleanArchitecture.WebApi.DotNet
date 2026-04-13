using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.CreatePermission;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Permissions.Commands;

[TestClass]
public class CreatePermissionCommandHandlerTests
{
    #pragma warning disable CS8618
    private IPermissionRepository _permissionRepository;
    private IUnitOfWork _unitOfWork;
    private CreatePermissionCommandHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreatePermissionCommandHandler(_permissionRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_Should_CreatePermission_When_CommandIsValid()
    {
        // Arrange
        var command = new CreatePermissionCommand("users.read", "Allows reading users");
        var created = Permission.Create(command.Name, command.Description);
        _permissionRepository.Add(Arg.Any<Permission>()).Returns(created);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        await _permissionRepository.Received(1).Add(Arg.Any<Permission>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [TestMethod]
    public async Task Handle_Should_Rollback_When_ExceptionIsThrown()
    {
        // Arrange
        var command = new CreatePermissionCommand("users.read", "Allows reading users");
        _permissionRepository.Add(Arg.Any<Permission>()).Throws(new Exception("Database error"));

        // Act
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command));

        // Assert
        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
    }
}
