using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetAllPermissions;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Permissions.Queries;

[TestClass]
public class GetAllPermissionsQueryHandlerTests
{
    #pragma warning disable CS8618
    private IPermissionRepository _permissionRepository;
    private GetAllPermissionsQueryHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _handler = new GetAllPermissionsQueryHandler(_permissionRepository);
    }

    [TestMethod]
    public async Task Handle_Returns_Paginated_Permissions()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            Permission.Create("users.read", "Allows reading users"),
            Permission.Create("users.write", "Allows writing users")
        };
        var paginated = new PaginatedList<Permission>(permissions, page: 1, pageSize: 10, totalCount: 2);

        _permissionRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<Permission, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        // Act
        var result = await _handler.Handle(new GetAllPermissionsQuery());

        // Assert
        Assert.HasCount(2, result.Items);
        Assert.AreEqual(1, result.Page);
        Assert.AreEqual(10, result.PageSize);
        Assert.AreEqual(2, result.TotalCount);
        Assert.IsFalse(result.HasNextPage);
        Assert.IsFalse(result.HasPreviousPage);
    }

    [TestMethod]
    public async Task Handle_Returns_Empty_When_No_Permissions()
    {
        // Arrange
        var empty = new PaginatedList<Permission>([], page: 1, pageSize: 10, totalCount: 0);

        _permissionRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<Permission, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(empty);

        // Act
        var result = await _handler.Handle(new GetAllPermissionsQuery());

        // Assert
        Assert.IsEmpty(result.Items);
        Assert.AreEqual(0, result.TotalCount);
    }

    [TestMethod]
    public async Task Handle_Filters_By_Search_Term()
    {
        // Arrange
        var permissions = new List<Permission> { Permission.Create("users.read", "Allows reading users") };
        var paginated = new PaginatedList<Permission>(permissions, page: 1, pageSize: 10, totalCount: 1);

        _permissionRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<Permission, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        // Act
        var result = await _handler.Handle(new GetAllPermissionsQuery(Search: "users"));

        // Assert
        Assert.HasCount(1, result.Items);
        Assert.AreEqual("users.read", result.Items[0].Name);
    }

    [TestMethod]
    public async Task Handle_Respects_Pagination_Parameters()
    {
        // Arrange
        var permissions = new List<Permission> { Permission.Create("roles.read", "Allows reading roles") };
        var paginated = new PaginatedList<Permission>(permissions, page: 2, pageSize: 1, totalCount: 3);

        _permissionRepository.GetPagedAsync(2, 1, Arg.Any<Expression<Func<Permission, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        // Act
        var result = await _handler.Handle(new GetAllPermissionsQuery(Page: 2, PageSize: 1));

        // Assert
        Assert.HasCount(1, result.Items);
        Assert.AreEqual(2, result.Page);
        Assert.AreEqual(1, result.PageSize);
        Assert.AreEqual(3, result.TotalCount);
        Assert.AreEqual(3, result.TotalPages);
        Assert.IsTrue(result.HasNextPage);
        Assert.IsTrue(result.HasPreviousPage);
    }
}
