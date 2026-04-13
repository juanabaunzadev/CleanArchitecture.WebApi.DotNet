using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.DTOs.Roles;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.Mappers;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleDetailResponse>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDetailResponse> Handle(GetRoleByIdQuery query, CancellationToken ct = default)
    {
        var role = await _roleRepository.GetByIdAsync(query.Id, ct);

        if (role is null)
            throw new NotFoundException();

        return RoleMapper.ToDetailResponse(role);
    }
}
