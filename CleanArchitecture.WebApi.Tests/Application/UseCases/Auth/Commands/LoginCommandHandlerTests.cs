using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Abstractions.Security;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;
using CleanArchitecture.WebApi.Domain.Entities;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Application.UseCases.Auth.Commands;

[TestClass]
public class LoginCommandHandlerTests
{
    #pragma warning disable CS8618
    private IUserRepository _userRepository;
    private IRefreshTokenRepository _refreshTokenRepository;
    private IUnitOfWork _unitOfWork;
    private IPasswordHasher _passwordHasher;
    private IJwtTokenService _jwtTokenService;
    private LoginCommandHandler _handler;
    #pragma warning restore CS8618

    [TestInitialize]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();
        _handler = new LoginCommandHandler(_userRepository, _refreshTokenRepository, _unitOfWork, _passwordHasher, _jwtTokenService);
    }

    [TestMethod]
    public async Task Handle_Should_ReturnToken_When_CredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand("john.doe@example.com", "Password123!");
        var user = User.Create("John", "Doe", command.Email, "hashed_password");
        var expectedResponse = new LoginResponse("jwt_token", DateTime.UtcNow.AddMinutes(60), "refresh_token", DateTime.UtcNow.AddDays(7));

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify(command.Password, user.PasswordHash).Returns(true);
        _jwtTokenService.GenerateToken(user, Arg.Any<string>(), Arg.Any<DateTime>()).Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResponse.Token, result.Token);
        Assert.IsNotNull(result.RefreshToken);
        _jwtTokenService.Received(1).GenerateToken(user, Arg.Any<string>(), Arg.Any<DateTime>());
    }

    [TestMethod]
    public async Task Handle_Should_ThrowUnauthorizedException_When_UserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand("notfound@example.com", "Password123!");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command));
        _jwtTokenService.DidNotReceive().GenerateToken(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<DateTime>());
    }

    [TestMethod]
    public async Task Handle_Should_ThrowUnauthorizedException_When_PasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand("john.doe@example.com", "WrongPassword!");
        var user = User.Create("John", "Doe", command.Email, "hashed_password");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify(command.Password, user.PasswordHash).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command));
        _jwtTokenService.DidNotReceive().GenerateToken(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<DateTime>());
    }
}
