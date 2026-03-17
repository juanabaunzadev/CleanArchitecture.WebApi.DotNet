using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.UpdateUser;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetAllUsersQuery, IReadOnlyList<UserResponse>>, GetAllUsersQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQuery, UserResponse>, GetUserByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        return services;
    }
}
