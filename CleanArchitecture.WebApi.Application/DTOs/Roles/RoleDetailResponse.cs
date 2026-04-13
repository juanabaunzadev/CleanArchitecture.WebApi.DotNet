using CleanArchitecture.WebApi.Application.DTOs.Permissions;

namespace CleanArchitecture.WebApi.Application.DTOs.Roles;

public record RoleDetailResponse(
    Guid Id,
    string Name,
    IEnumerable<PermissionResponse> Permissions
);
