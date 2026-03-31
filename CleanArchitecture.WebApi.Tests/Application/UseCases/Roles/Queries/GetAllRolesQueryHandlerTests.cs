using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetAllRoles;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Roles.Queries;

[TestClass]
public class GetAllRolesQueryHandlerTests
{
    #pragma warning disable CS8618
    private IRoleRepository _roleRepository;
    private GetAllRolesQueryHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _handler = new GetAllRolesQueryHandler(_roleRepository);
    }

    [TestMethod]
    public async Task Handle_Returns_Paginated_Roles()
    {
        // Arrange
        var roles = new List<Role>
        {
            Role.Create("Admin"),
            Role.Create("User")
        };
        var paginated = new PaginatedList<Role>(roles, page: 1, pageSize: 10, totalCount: 2);

        _roleRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<Role, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        // Act
        var result = await _handler.Handle(new GetAllRolesQuery());

        // Assert
        Assert.HasCount(2, result.Items);
        Assert.AreEqual(1, result.Page);
        Assert.AreEqual(10, result.PageSize);
        Assert.AreEqual(2, result.TotalCount);
        Assert.IsFalse(result.HasNextPage);
        Assert.IsFalse(result.HasPreviousPage);
    }

    [TestMethod]
    public async Task Handle_Returns_Empty_When_No_Roles()
    {
        // Arrange
        var empty = new PaginatedList<Role>([], page: 1, pageSize: 10, totalCount: 0);

        _roleRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<Role, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(empty);

        // Act
        var result = await _handler.Handle(new GetAllRolesQuery());

        // Assert
        Assert.IsEmpty(result.Items);
        Assert.AreEqual(0, result.TotalCount);
    }

    [TestMethod]
    public async Task Handle_Filters_By_Search_Term()
    {
        // Arrange
        var roles = new List<Role> { Role.Create("Admin") };
        var paginated = new PaginatedList<Role>(roles, page: 1, pageSize: 10, totalCount: 1);

        _roleRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<Role, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        // Act
        var result = await _handler.Handle(new GetAllRolesQuery(Search: "Admin"));

        // Assert
        Assert.HasCount(1, result.Items);
        Assert.AreEqual("Admin", result.Items[0].Name);
    }

    [TestMethod]
    public async Task Handle_Respects_Pagination_Parameters()
    {
        // Arrange
        var roles = new List<Role> { Role.Create("Moderator") };
        var paginated = new PaginatedList<Role>(roles, page: 2, pageSize: 1, totalCount: 3);

        _roleRepository.GetPagedAsync(2, 1, Arg.Any<Expression<Func<Role, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginated);

        // Act
        var result = await _handler.Handle(new GetAllRolesQuery(Page: 2, PageSize: 1));

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
