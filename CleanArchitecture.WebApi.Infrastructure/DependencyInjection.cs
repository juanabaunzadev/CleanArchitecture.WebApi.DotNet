using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Infrastructure.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IMediator, AppMediator>();

        return services;
    }
}
