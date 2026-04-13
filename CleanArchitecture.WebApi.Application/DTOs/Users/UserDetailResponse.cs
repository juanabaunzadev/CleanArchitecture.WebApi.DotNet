using CleanArchitecture.WebApi.Application.DTOs.Roles;

namespace CleanArchitecture.WebApi.Application.DTOs.Users;

public record UserDetailResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<RoleResponse> Roles
);
