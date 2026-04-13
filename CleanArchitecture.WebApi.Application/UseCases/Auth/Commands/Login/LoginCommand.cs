using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
