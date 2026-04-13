using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Permissions;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery(
    int Page = PaginationDefaults.DefaultPage,
    int PageSize = PaginationDefaults.DefaultPageSize,
    string? Search = null
) : PagedQuery<PermissionResponse>(Page, PageSize, Search);
