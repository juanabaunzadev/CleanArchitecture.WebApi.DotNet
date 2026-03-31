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

    private static void VerifyDomainRules(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name is required.");
    }
}
