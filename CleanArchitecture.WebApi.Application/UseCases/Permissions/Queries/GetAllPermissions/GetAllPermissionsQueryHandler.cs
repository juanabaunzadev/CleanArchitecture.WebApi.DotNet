using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Permissions;
using CleanArchitecture.WebApi.Application.Mappers;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetAllPermissions;

public class GetAllPermissionsQueryHandler : IQueryHandler<GetAllPermissionsQuery, PaginatedList<PermissionResponse>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<PaginatedList<PermissionResponse>> Handle(GetAllPermissionsQuery query, CancellationToken ct = default)
    {
        Expression<Func<Permission, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            filter = p => p.Name.ToLower().Contains(search) ||
                          p.Description.ToLower().Contains(search);
        }

        var paginated = await _permissionRepository.GetPagedAsync(query.Page, query.PageSize, filter, ct);
        var mappedItems = paginated.Items.Select(PermissionMapper.ToResponse);

        return new PaginatedList<PermissionResponse>(mappedItems, paginated.Page, paginated.PageSize, paginated.TotalCount);
    }
}
