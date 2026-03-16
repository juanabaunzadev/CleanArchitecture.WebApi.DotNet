namespace CleanArchitecture.WebApi.Application.DTOs.Users;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAt
);