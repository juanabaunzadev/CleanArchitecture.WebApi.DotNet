namespace CleanArchitecture.WebApi.Application.Abstractions.Mediator;

public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command);
}