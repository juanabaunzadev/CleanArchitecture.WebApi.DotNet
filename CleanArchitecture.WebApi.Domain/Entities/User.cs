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
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("User first name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("User last name is required.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("User password hash is required.");

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
}
