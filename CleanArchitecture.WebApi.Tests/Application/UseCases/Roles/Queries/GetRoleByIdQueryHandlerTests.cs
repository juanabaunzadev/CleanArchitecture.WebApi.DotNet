using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.DTOs.Roles;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetRoleById;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Roles.Queries;

[TestClass]
public class GetRoleByIdQueryHandlerTests
{
    #pragma warning disable CS8618
    private IRoleRepository _roleRepository;
    private GetRoleByIdQueryHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _handler = new GetRoleByIdQueryHandler(_roleRepository);
    }

    [TestMethod]
    public async Task Handle_Returns_RoleDetailResponse_When_Role_Exists()
    {
        // Arrange
        var role = Role.Create("Admin");
        var query = new GetRoleByIdQuery(role.Id);
        _roleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns(role);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<RoleDetailResponse>(result);
        Assert.AreEqual(role.Id, result.Id);
        Assert.AreEqual(role.Name, result.Name);
        Assert.AreEqual(0, result.Permissions.Count());
    }

    [TestMethod]
    public async Task Handle_Throws_NotFoundException_When_Role_Does_Not_Exist()
    {
        // Arrange
        var query = new GetRoleByIdQuery(Guid.NewGuid());
        _roleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns((Role)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query));
    }
}
