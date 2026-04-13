namespace CleanArchitecture.WebApi.Application.DTOs.Permissions;

public record PermissionResponse(
    Guid Id,
    string Name,
    string Description
);
