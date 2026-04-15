using CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.Abstractions.Security;

public interface IJwtTokenService
{
    LoginResponse GenerateToken(User user, string refreshToken, DateTime refreshTokenExpiry);
}
