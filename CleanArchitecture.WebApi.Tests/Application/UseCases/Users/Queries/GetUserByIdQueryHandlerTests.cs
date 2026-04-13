using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetUserById;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Users.Queries;

[TestClass]
public class GetUserByIdQueryHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IUserRepository _userRepository;
    private GetUserByIdQueryHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_userRepository);
    }

    [TestMethod]
    public async Task Handle_Returns_UserDetailResponse_When_User_Exists()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john.doe@example.com", "password");
        var id = user.Id;
        var query = new GetUserByIdQuery(id);
        
        _userRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Id, result.Id);
        Assert.AreEqual(user.FirstName, result.FirstName);
        Assert.AreEqual(user.LastName, result.LastName);
        Assert.AreEqual(user.Email.Value, result.Email);
        Assert.AreEqual(user.IsActive, result.IsActive);
    }

    [TestMethod]
    public async Task Handle_Throws_NotFoundException_When_User_Does_Not_Exist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetUserByIdQuery(id);

        _userRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query));
    }
}
