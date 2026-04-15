using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Abstractions.Security;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;
using CleanArchitecture.WebApi.Domain.Entities;
using RefreshTokenEntity = CleanArchitecture.WebApi.Domain.Entities.RefreshToken;

namespace CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand command, CancellationToken ct = default)
    {
        var existing = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, ct);

        if (existing is null || !existing.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        existing.Revoke();

        var newToken = Guid.NewGuid().ToString();
        var newExpiry = DateTime.UtcNow.AddDays(7);
        var newRefreshToken = RefreshTokenEntity.Create(existing.UserId, newToken, newExpiry);

        try
        {
            await _refreshTokenRepository.AddAsync(newRefreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }

        return _jwtTokenService.GenerateToken(existing.User, newToken, newExpiry);
    }
}
