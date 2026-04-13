using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.UpdateRole;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Roles.Commands;

[TestClass]
public class UpdateRoleCommandHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IRoleRepository _roleRepository;
    private IUnitOfWork _unitOfWork;
    private UpdateRoleCommandHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateRoleCommandHandler(_roleRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_Should_UpdateRole_When_CommandIsValid()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new UpdateRoleCommand(roleId, "Admin", [Guid.NewGuid()]);
        var existingRole = Role.Create("User");
        _roleRepository.GetByIdAsync(roleId, Arg.Any<CancellationToken>()).Returns(existingRole);

        // Act
        await _handler.Handle(command);

        // Assert
        await _roleRepository.Received(1).Update(Arg.Any<Role>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_Should_NotUpdate_When_RoleDoesNotExist()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new UpdateRoleCommand(roleId, "Admin", []);
        _roleRepository.GetByIdAsync(roleId, Arg.Any<CancellationToken>()).Returns((Role)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command));
        await _roleRepository.DidNotReceive().Update(Arg.Any<Role>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
