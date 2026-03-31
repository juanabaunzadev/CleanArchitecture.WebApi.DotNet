using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id) : ICommand;
