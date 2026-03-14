using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    private UserRole() { }

    public static UserRole Create(Guid userId, Guid roleId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserRole userId cannot be empty.");

        if (roleId == Guid.Empty)
            throw new DomainException("UserRole roleId cannot be empty.");

        return new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}
