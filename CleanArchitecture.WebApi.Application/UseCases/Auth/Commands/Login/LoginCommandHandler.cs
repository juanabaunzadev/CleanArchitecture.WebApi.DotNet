using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Abstractions.Security;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Domain.Entities;
using RefreshTokenEntity = CleanArchitecture.WebApi.Domain.Entities.RefreshToken;

namespace CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, ct);

        if (user is null || !_passwordHasher.Verify(command.Password, user.PasswordHash) || !user.IsActive)
            throw new UnauthorizedException("Invalid credentials.");

        var token = Guid.NewGuid().ToString();
        var expiry = DateTime.UtcNow.AddDays(7);
        var refreshToken = RefreshTokenEntity.Create(user.Id, token, expiry);

        try
        {
            await _refreshTokenRepository.AddAsync(refreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }

        return _jwtTokenService.GenerateToken(user, token, expiry);
    }
}
