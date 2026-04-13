using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public ICollection<UserRole> UserRoles { get; private set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; private set; } = null!;

    private Role() { }

    public static Role Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name is required.");

        return new Role
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            UserRoles = new List<UserRole>(),
            RolePermissions = new List<RolePermission>()
        };
    }

    public void Update(string name)
    {
        VerifyDomainRules(name);

        Name = name.Trim();
    }

    public void SyncPermissions(List<Guid> permissionIds)
    {
        var toRemove = RolePermissions
            .Where(rp => !permissionIds.Contains(rp.PermissionId))
            .ToList();

        foreach (var rp in toRemove)
            RolePermissions.Remove(rp);

        var existingIds = RolePermissions.Select(rp => rp.PermissionId).ToList();
        var toAdd = permissionIds.Where(id => !existingIds.Contains(id));

        foreach (var id in toAdd)
            RolePermissions.Add(RolePermission.Create(Id, id));
    }

    private static void VerifyDomainRules(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name is required.");
    }
}
