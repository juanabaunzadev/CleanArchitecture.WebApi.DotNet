using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    List<Guid> RoleIds) : ICommand;