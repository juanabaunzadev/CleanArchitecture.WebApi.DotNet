using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Roles;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQuery(
    int Page = PaginationDefaults.DefaultPage,
    int PageSize = PaginationDefaults.DefaultPageSize,
    string? Search = null
) : PagedQuery<RoleResponse>(Page, PageSize, Search);
