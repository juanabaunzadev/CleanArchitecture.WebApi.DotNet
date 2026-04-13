using CleanArchitecture.WebApi.Domain.Exceptions;
using CleanArchitecture.WebApi.Domain.ValueObjects;

namespace CleanArchitecture.WebApi.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<UserRole> UserRoles { get; private set; } = null!;

    private User() { }

    public string GetFullName() => $"{FirstName} {LastName}";

    public static User Create(string firstName, string lastName, string email, string passwordHash)
    {
        VerifyDomainRules(firstName, lastName, email, passwordHash);

        return new User
        {
            Id = Guid.CreateVersion7(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = Email.Create(email),
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UserRoles = new List<UserRole>()
        };
    }

    public void SyncRoles(List<Guid> roleIds)
    {
        var toRemove = UserRoles
            .Where(ur => !roleIds.Contains(ur.RoleId))
            .ToList();

        foreach (var ur in toRemove)
            UserRoles.Remove(ur);

        var existingIds = UserRoles.Select(ur => ur.RoleId).ToList();
        var toAdd = roleIds.Where(id => !existingIds.Contains(id));

        foreach (var id in toAdd)
            UserRoles.Add(UserRole.Create(Id, id));
    }

    public void Update(string firstName, string lastName, string email)
    {
        VerifyDomainRules(firstName, lastName, email);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = Email.Create(email);
    }

    private static void VerifyDomainRules(string firstName, string lastName, string email, string? passwordHash = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("User first name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("User last name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("User email is required.");

        if (passwordHash is not null && string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("User password hash is required.");
    }
}
