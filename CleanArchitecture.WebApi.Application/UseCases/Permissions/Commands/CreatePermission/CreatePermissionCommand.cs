using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.CreatePermission;

public sealed record CreatePermissionCommand(string Name, string Description) : ICommand<Guid>;
