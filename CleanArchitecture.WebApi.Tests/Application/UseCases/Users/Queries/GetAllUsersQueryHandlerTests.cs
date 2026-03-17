using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
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
    public void Handle_Returns_All_Users()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create("John", "Doe", "john.doe@example.com", "password123"),
            User.Create("Jane", "Smith", "jane.smith@example.com", "password123")
        };

        _userRepository.GetAllAsync().Returns(users);

        // Act
        var result = _handler.Handle(new GetAllUsersQuery()).Result;

        // Assert
        Assert.HasCount(users.Count, result);
        for (int i = 0; i < users.Count; i++)
        {
            Assert.AreEqual(users[i].Id, result[i].Id);
            Assert.AreEqual(users[i].FirstName, result[i].FirstName);
            Assert.AreEqual(users[i].LastName, result[i].LastName);
            Assert.AreEqual(users[i].Email.Value, result[i].Email);
            Assert.AreEqual(users[i].IsActive, result[i].IsActive);
            Assert.AreEqual(users[i].CreatedAt, result[i].CreatedAt);
        }
    }

    [TestMethod]
    public void Handle_Returns_Empty_List_When_No_Users()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(new List<User>());

        // Act
        var result = _handler.Handle(new GetAllUsersQuery()).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }
}
