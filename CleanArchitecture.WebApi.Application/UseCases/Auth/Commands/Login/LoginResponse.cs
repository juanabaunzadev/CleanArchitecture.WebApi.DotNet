namespace CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;

public record LoginResponse(
    string Token,
    DateTime ExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiry
);
