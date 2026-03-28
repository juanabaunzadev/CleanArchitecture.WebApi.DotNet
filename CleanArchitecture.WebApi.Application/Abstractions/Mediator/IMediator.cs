namespace CleanArchitecture.WebApi.Application.Abstractions.Mediator;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
    Task Send(ICommand command, CancellationToken ct = default);
}