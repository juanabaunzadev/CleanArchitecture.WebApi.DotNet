using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Abstractions.Security;
using CleanArchitecture.WebApi.Infrastructure.Mediator;
using CleanArchitecture.WebApi.Infrastructure.Repositories;
using CleanArchitecture.WebApi.Infrastructure.Security;
using CleanArchitecture.WebApi.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer("name=AppConnectionString");
        });

        services.AddSingleton<IPasswordHasher, Argon2idPasswordHasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddTransient<IMediator, AppMediator>();
        services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}
