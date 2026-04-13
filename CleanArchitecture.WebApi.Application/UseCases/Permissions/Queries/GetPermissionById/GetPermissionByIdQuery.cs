using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.DTOs.Permissions;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetPermissionById;

public sealed record GetPermissionByIdQuery(Guid Id) : IQuery<PermissionResponse>;
