using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.DeleteRole;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Roles.Commands;

[TestClass]
public class DeleteRoleCommandHandlerTests
{
    #pragma warning disable CS8618
    private IRoleRepository _roleRepository;
    private IUnitOfWork _unitOfWork;
    private DeleteRoleCommandHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteRoleCommandHandler(_roleRepository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_Should_DeleteRole_When_CommandIsValid()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new DeleteRoleCommand(roleId);
        var existingRole = Role.Create("Admin");
        _roleRepository.GetByIdAsync(roleId, Arg.Any<CancellationToken>()).Returns(existingRole);

        // Act
        await _handler.Handle(command);

        // Assert
        await _roleRepository.Received(1).Delete(existingRole);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_Should_NotDelete_When_RoleDoesNotExist()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new DeleteRoleCommand(roleId);
        _roleRepository.GetByIdAsync(roleId, Arg.Any<CancellationToken>()).Returns((Role)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command));
        await _roleRepository.DidNotReceive().Delete(Arg.Any<Role>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
