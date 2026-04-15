using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.WebApi.Application.Abstractions.Security;
using CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;
using CleanArchitecture.WebApi.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.WebApi.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;

    public JwtTokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"]!;
        _issuer = configuration["Jwt:Issuer"]!;
        _audience = configuration["Jwt:Audience"]!;
        _expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"]!);
    }

    public LoginResponse GenerateToken(User user, string refreshToken, DateTime refreshTokenExpiry)
    {
        var claims = BuildClaims(user);
        var expiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, refreshToken, refreshTokenExpiry);
    }

    private static List<Claim> BuildClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = user.UserRoles.Select(ur => ur.Role.Name).Distinct();
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct();

        foreach (var permission in permissions)
            claims.Add(new Claim("permission", permission));

        return claims;
    }
}
