using CleanArchitecture.WebApi.Application.Abstractions.Mediator;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : ICommand;
