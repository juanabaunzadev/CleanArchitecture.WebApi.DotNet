namespace CleanArchitecture.WebApi.Application.Abstractions.Mediator;

public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken ct = default);
}