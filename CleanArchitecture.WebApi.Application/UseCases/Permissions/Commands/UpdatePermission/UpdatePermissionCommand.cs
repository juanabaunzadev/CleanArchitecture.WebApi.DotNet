using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.UpdatePermission;

public sealed record UpdatePermissionCommand(Guid Id, string Name, string Description) : ICommand;
