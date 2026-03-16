namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
);