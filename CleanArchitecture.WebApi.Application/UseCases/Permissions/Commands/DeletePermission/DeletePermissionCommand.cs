using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.DeletePermission;

public sealed record DeletePermissionCommand(Guid Id) : ICommand;
