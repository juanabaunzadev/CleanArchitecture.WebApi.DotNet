using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
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
        var validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
        var validator = _serviceProvider.GetService(validatorType);

        if (validator is IValidator validatorInstance)
        {
            var context = new ValidationContext<object>(command);
            var validationResult = await validatorInstance.ValidateAsync(context);
            
            if (!validationResult.IsValid)
                throw new AppValidationException(validationResult);
        }

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
        var validatorType = typeof(IValidator<>).MakeGenericType(query.GetType());
        var validator = _serviceProvider.GetService(validatorType);

        if (validator is IValidator validatorInstance)
        {
            var context = new ValidationContext<object>(query);
            var validationResult = await validatorInstance.ValidateAsync(context);

            if (!validationResult.IsValid)
                throw new AppValidationException(validationResult);
        }
        
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new MediatorException($"No handler registered for {query.GetType().Name}.");

        return await (Task<TResponse>)handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResponse>, TResponse>.Handle))!
            .Invoke(handler, [query])!;
    }
}
