using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Users;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(
    int Page = PaginationDefaults.DefaultPage,
    int PageSize = PaginationDefaults.DefaultPageSize,
    string? Search = null
) : PagedQuery<UserResponse>(Page, PageSize, Search);