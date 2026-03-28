using Microsoft.VisualStudio.TestTools.UnitTesting;
using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Infrastructure.Mediator;
using FluentValidation;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Infrastructure.Mediator;

[TestClass]
public class AppMediatorTests
{
    // Fake Command
    public record FakeCommand : ICommand<Guid>
    {
        public required string Name { get; set; }
    };

    // Fake Handler
    public class FakeCommandHandler : ICommandHandler<FakeCommand, Guid>
    {
        public Task<Guid> Handle(FakeCommand command, CancellationToken ct = default)
        {
            return Task.FromResult(Guid.CreateVersion7());
        }
    }

    // Fake Validator
    public class FakeCommandValidator : AbstractValidator<FakeCommand>
    {
        public FakeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly AppMediator _mediator;

    public AppMediatorTests()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _mediator = new AppMediator(_serviceProvider);
    }

    [TestMethod]
    public async Task Send_ShouldCallHandler_WhenCommandHandlerIsRegistered()
    {
        // Arrange
        var command = new FakeCommand(){ Name = "Test Command" };
        var expectedId = Guid.CreateVersion7();

        var handler = Substitute.For<ICommandHandler<FakeCommand, Guid>>();
        handler.Handle(command, Arg.Any<CancellationToken>()).Returns(expectedId);

        _serviceProvider.GetService(typeof(ICommandHandler<FakeCommand, Guid>))
            .Returns(handler);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        await handler.Received(1).Handle(command, Arg.Any<CancellationToken>());
        Assert.AreEqual(expectedId, result);
    }

    [TestMethod]
    public async Task Send_ShouldThrowMediatorException_WhenHandlerIsNotRegistered()
    {
        // Arrange
        var command = new FakeCommand(){ Name = "Test Command" };

        _serviceProvider.GetService(Arg.Any<Type>()).Returns(null!);

        // Act
        var act = async () => await _mediator.Send(command);

        // Assert
        await Assert.ThrowsAsync<MediatorException>(act);
    }

    [TestMethod]
    public async Task Send_ShouldThrowAppValidationException_WhenValidationFails()
    {
        // Arrange
        var command = new FakeCommand(){ Name = "" };
        var validator = new FakeCommandValidator();
        
        _serviceProvider.GetService(typeof(IValidator<FakeCommand>))
            .Returns(validator);

        // Act
        var act = async () => await _mediator.Send(command);

        // Assert
        await Assert.ThrowsAsync<AppValidationException>(act);
    }
}
