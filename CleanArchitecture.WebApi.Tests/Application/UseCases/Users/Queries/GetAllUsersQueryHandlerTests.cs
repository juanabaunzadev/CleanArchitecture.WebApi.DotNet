using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Users.Queries;

[TestClass]
public class GetAllUsersQueryHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IUserRepository _userRepository;
    private GetAllUsersQueryHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new GetAllUsersQueryHandler(_userRepository);
    }

    [TestMethod]
    public void Handle_Returns_Paged_Users()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("John", "Doe", "john.doe@example.com", "password123"),
            User.Create("Jane", "Smith", "jane.smith@example.com", "password123")
        };

        var paginatedUsers = new PaginatedList<User>(users, page: 1, pageSize: 10, totalCount: 2);

        _userRepository.GetPagedAsync(1, 10, Arg.Any<Specification<User>>()).Returns(paginatedUsers);

        // Act
        var result = _handler.Handle(new GetAllUsersQuery(1, 10)).Result;

        // Assert
        Assert.HasCount(users.Count, result.Items);
        for (int i = 0; i < users.Count; i++)
        {
            Assert.AreEqual(users[i].Id, result.Items[i].Id);
            Assert.AreEqual(users[i].FirstName, result.Items[i].FirstName);
            Assert.AreEqual(users[i].LastName, result.Items[i].LastName);
            Assert.AreEqual(users[i].Email.Value, result.Items[i].Email);
            Assert.AreEqual(users[i].IsActive, result.Items[i].IsActive);
            Assert.AreEqual(users[i].CreatedAt, result.Items[i].CreatedAt);
        }
    }

    [TestMethod]
    public void Handle_Returns_Empty_List_When_No_Users()
    {
        // Arrange
        var paginatedUsers = new PaginatedList<User>(new List<User>(), page: 1, pageSize: 10, totalCount: 0);

        _userRepository.GetPagedAsync(1, 10, Arg.Any<Specification<User>>()).Returns(paginatedUsers);

        // Act
        var result = _handler.Handle(new GetAllUsersQuery(1, 10)).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result.Items);
    }

    [TestMethod]
    public void Handle_Returns_Filtered_Users_By_FirstName()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("John", "Doe", "john.doe@example.com", "password123")
        };

        var paginatedUsers = new PaginatedList<User>(users, page: 1, pageSize: 10, totalCount: 1);

        _userRepository.GetPagedAsync(1, 10, Arg.Any<Specification<User>>()).Returns(paginatedUsers);

        // Act
        var result = _handler.Handle(new GetAllUsersQuery(1, 10, FirstName: "John")).Result;

        // Assert
        Assert.HasCount(1, result.Items);
        Assert.AreEqual("John", result.Items[0].FirstName);
    }
}
