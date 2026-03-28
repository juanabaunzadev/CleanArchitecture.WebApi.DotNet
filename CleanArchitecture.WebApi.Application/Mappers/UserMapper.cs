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

    public static IEnumerable<UserResponse> ToResponseList(IEnumerable<User> users)
        => users.Select(ToResponse);
}
