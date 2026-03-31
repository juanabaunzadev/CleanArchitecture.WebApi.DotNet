using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Roles;
using CleanArchitecture.WebApi.Application.Mappers;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetAllRoles;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, PaginatedList<RoleResponse>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<PaginatedList<RoleResponse>> Handle(GetAllRolesQuery query, CancellationToken ct = default)
    {
        Expression<Func<Role, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            filter = r => r.Name.ToLower().Contains(search);
        }

        var paginatedRoles = await _roleRepository.GetPagedAsync(query.Page, query.PageSize, filter, ct);
        var mappedItems = paginatedRoles.Items.Select(RoleMapper.ToResponse);

        return new PaginatedList<RoleResponse>(mappedItems, paginatedRoles.Page, paginatedRoles.PageSize, paginatedRoles.TotalCount);
    }
}
