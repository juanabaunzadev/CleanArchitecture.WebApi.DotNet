using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Domain.ValueObjects;

public record Email
{
    public string Value { get; private set; } = null!;

    public Email() { }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if(!value.Contains("@"))
            throw new DomainException("Email must contain '@'.");

        return new Email { Value = value.ToLower().Trim() };
    }
}
