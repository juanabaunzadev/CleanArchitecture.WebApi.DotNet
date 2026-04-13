using CleanArchitecture.WebApi.Application.DTOs.Permissions;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.Mappers;

public static class PermissionMapper
{
    public static PermissionResponse ToResponse(Permission permission)
    {
        return new PermissionResponse(
            permission.Id,
            permission.Name,
            permission.Description
        );
    }
}
