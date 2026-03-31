using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(Guid Id, string Name) : ICommand;