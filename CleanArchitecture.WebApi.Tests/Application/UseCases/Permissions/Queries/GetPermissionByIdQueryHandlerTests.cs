using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetPermissionById;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Permissions.Queries;

[TestClass]
public class GetPermissionByIdQueryHandlerTests
{
    #pragma warning disable CS8618
    private IPermissionRepository _permissionRepository;
    private GetPermissionByIdQueryHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _handler = new GetPermissionByIdQueryHandler(_permissionRepository);
    }

    [TestMethod]
    public async Task Handle_Returns_PermissionResponse_When_Permission_Exists()
    {
        // Arrange
        var permission = Permission.Create("users.read", "Allows reading users");
        var query = new GetPermissionByIdQuery(permission.Id);
        _permissionRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns(permission);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(permission.Id, result.Id);
        Assert.AreEqual(permission.Name, result.Name);
        Assert.AreEqual(permission.Description, result.Description);
    }

    [TestMethod]
    public async Task Handle_Throws_NotFoundException_When_Permission_Does_Not_Exist()
    {
        // Arrange
        var query = new GetPermissionByIdQuery(Guid.NewGuid());
        _permissionRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns((Permission)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query));
    }
}
