using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;

public sealed record CreateRoleCommand(string Name) : ICommand<Guid>;
