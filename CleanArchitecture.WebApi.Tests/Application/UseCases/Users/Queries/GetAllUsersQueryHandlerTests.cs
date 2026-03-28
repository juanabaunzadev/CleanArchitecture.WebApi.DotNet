using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Users.Queries;

[TestClass]
public class GetAllUsersQueryHandlerTests
{
    #pragma warning disable CS8618
    private IUserRepository _userRepository;
    private GetAllUsersQueryHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new GetAllUsersQueryHandler(_userRepository);
    }

    [TestMethod]
    public async Task Handle_Returns_Paginated_Users()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("John", "Doe", "john.doe@example.com", "password123"),
            User.Create("Jane", "Smith", "jane.smith@example.com", "password123")
        };
        var paginatedUsers = new PaginatedList<User>(users, page: 1, pageSize: 10, totalCount: 2);

        _userRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<User, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginatedUsers);

        // Act
        var result = await _handler.Handle(new GetAllUsersQuery());

        // Assert
        Assert.HasCount(2, result.Items);
        Assert.AreEqual(1, result.Page);
        Assert.AreEqual(10, result.PageSize);
        Assert.AreEqual(2, result.TotalCount);
        Assert.IsFalse(result.HasNextPage);
        Assert.IsFalse(result.HasPreviousPage);
    }

    [TestMethod]
    public async Task Handle_Returns_Empty_When_No_Users()
    {
        // Arrange
        var empty = new PaginatedList<User>([], page: 1, pageSize: 10, totalCount: 0);

        _userRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<User, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(empty);

        // Act
        var result = await _handler.Handle(new GetAllUsersQuery());

        // Assert
        Assert.IsEmpty(result.Items);
        Assert.AreEqual(0, result.TotalCount);
    }

    [TestMethod]
    public async Task Handle_Filters_By_Search_Term()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("John", "Doe", "john.doe@example.com", "password123")
        };
        var paginatedUsers = new PaginatedList<User>(users, page: 1, pageSize: 10, totalCount: 1);

        _userRepository.GetPagedAsync(1, 10, Arg.Any<Expression<Func<User, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginatedUsers);

        // Act
        var result = await _handler.Handle(new GetAllUsersQuery(Search: "John"));

        // Assert
        Assert.HasCount(1, result.Items);
        Assert.AreEqual("John", result.Items[0].FirstName);
    }

    [TestMethod]
    public async Task Handle_Respects_Pagination_Parameters()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("Jane", "Smith", "jane.smith@example.com", "password123")
        };
        var paginatedUsers = new PaginatedList<User>(users, page: 2, pageSize: 1, totalCount: 3);

        _userRepository.GetPagedAsync(2, 1, Arg.Any<Expression<Func<User, bool>>?>(), Arg.Any<CancellationToken>())
            .Returns(paginatedUsers);

        // Act
        var result = await _handler.Handle(new GetAllUsersQuery(Page: 2, PageSize: 1));

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
