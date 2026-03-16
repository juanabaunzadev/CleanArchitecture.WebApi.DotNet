using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Infrastructure.Mediator;
using NSubstitute;

namespace CleanArchitecture.WebApi.Tests.Infrastructure.Mediator;

[TestClass]
public class AppMediatorTests
{
    // Fake Command
    public record FakeCommand : ICommand<Guid>;

    // Fake Handler
    public class FakeCommandHandler : ICommandHandler<FakeCommand, Guid>
    {
        public Task<Guid> Handle(FakeCommand command)
        {
            return Task.FromResult(Guid.CreateVersion7());
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
        var command = new FakeCommand();
        var expectedId = Guid.CreateVersion7();

        var handler = Substitute.For<ICommandHandler<FakeCommand, Guid>>();
        handler.Handle(command).Returns(expectedId);

        _serviceProvider.GetService(typeof(ICommandHandler<FakeCommand, Guid>))
            .Returns(handler);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        await handler.Received(1).Handle(command);
        Assert.AreEqual(expectedId, result);
    }

    [TestMethod]
    public async Task Send_ShouldThrowMediatorException_WhenHandlerIsNotRegistered()
    {
        // Arrange
        var command = new FakeCommand();

        _serviceProvider.GetService(Arg.Any<Type>()).Returns(null!);

        // Act
        var act = async () => await _mediator.Send(command);

        // Assert
        await Assert.ThrowsAsync<MediatorException>(act);
    }
}
