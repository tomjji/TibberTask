namespace TibberDeveloperTest.Application.Abstractions.Messaging;

public interface IBaseCommand;
public interface ICommand : IBaseCommand;
public interface ICommand<TResponse> : IBaseCommand;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Task> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}