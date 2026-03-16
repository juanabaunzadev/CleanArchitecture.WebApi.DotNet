using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebApi.Infrastructure.Mediator;

public class AppMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public AppMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new MediatorException($"No handler registered for {command.GetType().Name}.");

        return await (Task<TResponse>)handlerType
            .GetMethod(nameof(ICommandHandler<ICommand<TResponse>, TResponse>.Handle))!
            .Invoke(handler, [command])!;
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new MediatorException($"No handler registered for {query.GetType().Name}.");

        return await (Task<TResponse>)handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResponse>, TResponse>.Handle))!
            .Invoke(handler, [query])!;
    }
}
