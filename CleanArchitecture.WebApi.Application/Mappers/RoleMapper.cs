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
}
