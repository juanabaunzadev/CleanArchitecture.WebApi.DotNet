using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.UpdatePermission;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Permissions.Commands;

[TestClass]
public class UpdatePermissionCommandHandlerTests
{
    #pragma warning disable CS8618
    private IPermissionRepository _permissionRepository;
    private IUnitOfWork _unitOfWork;
    private UpdatePermissionCommandHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdatePermissionCommandHandler(_permissionRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_Should_UpdatePermission_When_CommandIsValid()
    {
        // Arrange
        var permissionId = Guid.NewGuid();
        var command = new UpdatePermissionCommand(permissionId, "users.write", "Allows writing users");
        var existing = Permission.Create("users.read", "Allows reading users");
        _permissionRepository.GetByIdAsync(permissionId, Arg.Any<CancellationToken>()).Returns(existing);

        // Act
        await _handler.Handle(command);

        // Assert
        await _permissionRepository.Received(1).Update(Arg.Any<Permission>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_Should_NotUpdate_When_PermissionDoesNotExist()
    {
        // Arrange
        var permissionId = Guid.NewGuid();
        var command = new UpdatePermissionCommand(permissionId, "users.write", "Allows writing users");
        _permissionRepository.GetByIdAsync(permissionId, Arg.Any<CancellationToken>()).Returns((Permission)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command));
        await _permissionRepository.DidNotReceive().Update(Arg.Any<Permission>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
