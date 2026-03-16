using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserCommandHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQuery, UserResponse>, GetUserByIdQueryHandler>();

        return services;
    }
}
