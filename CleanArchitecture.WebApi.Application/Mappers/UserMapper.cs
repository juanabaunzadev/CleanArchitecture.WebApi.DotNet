using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email.Value,
            user.IsActive,
            user.CreatedAt
        );
    }

    public static IReadOnlyList<UserResponse> ToResponseList(IEnumerable<User> users)
    {
        return users.Select(ToResponse).ToList();
    }
}
