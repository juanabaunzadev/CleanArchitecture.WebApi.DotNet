namespace CleanArchitecture.WebApi.Application.Abstractions.Mediator;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command);

    Task<TResponse> Send<TResponse>(IQuery<TResponse> query);
}