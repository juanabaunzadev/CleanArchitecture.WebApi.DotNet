using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Domain.Entities;

public class RolePermission
{
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public Guid PermissionId { get; private set; }
    public Permission Permission { get; private set; } = null!;

    private RolePermission() { }

    public static RolePermission Create(Guid roleId, Guid permissionId)
    {
        if (roleId == Guid.Empty)
            throw new DomainException("RolePermission roleId cannot be empty.");

        if (permissionId == Guid.Empty)
            throw new DomainException("RolePermission permissionId cannot be empty.");

        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };
    }
}
