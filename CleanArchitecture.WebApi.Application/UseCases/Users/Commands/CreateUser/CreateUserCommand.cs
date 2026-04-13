using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    List<Guid> RoleIds) : ICommand<Guid>;