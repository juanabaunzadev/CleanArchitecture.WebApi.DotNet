using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Domain.Entities;

public class Permission
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; private set; } = null!;

    private Permission() { }

    public static Permission Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Permission name is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Permission description is required.");

        return new Permission
        {
            Id = Guid.CreateVersion7(),
            Name = name.ToLower().Trim(),
            Description = description.Trim(),
            RolePermissions = new List<RolePermission>()
        };
    }

    public void Update(string name, string description)
    {
        VerifyDomainRules(name, description);

        Name = name.ToLower().Trim();
        Description = description.Trim();
    }

    private static void VerifyDomainRules(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Permission name is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Permission description is required.");
    }
}
