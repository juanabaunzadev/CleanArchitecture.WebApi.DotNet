using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.Common;

public abstract record PagedQuery<TResponse>(
    int Page = PaginationDefaults.DefaultPage,
    int PageSize = PaginationDefaults.DefaultPageSize,
    string? Search = null
) : IQuery<PaginatedList<TResponse>>;
