using CleanArchitecture.WebApi.Application.DTOs.Roles;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.Mappers;

public static class RoleMapper
{
    public static RoleResponse ToResponse(Role role)
    {
        return new RoleResponse(
            role.Id,
            role.Name
        );
    }

    public static RoleDetailResponse ToDetailResponse(Role role)
    {
        return new RoleDetailResponse(
            role.Id,
            role.Name,
            role.RolePermissions.Select(rp => PermissionMapper.ToResponse(rp.Permission))
        );
    }
}
