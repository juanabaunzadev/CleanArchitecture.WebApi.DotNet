using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.DTOs.Permissions;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.Mappers;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetPermissionById;

public class GetPermissionByIdQueryHandler : IQueryHandler<GetPermissionByIdQuery, PermissionResponse>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<PermissionResponse> Handle(GetPermissionByIdQuery query, CancellationToken ct = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(query.Id, ct);

        if (permission is null)
            throw new NotFoundException();

        return PermissionMapper.ToResponse(permission);
    }
}
