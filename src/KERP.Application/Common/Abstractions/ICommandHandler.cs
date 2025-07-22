﻿namespace KERP.Application.Common.Abstractions;

/// <summary>
/// Definiuje handlera dla komendy zwracającej rezultat.
/// </summary>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Definiuje handlera dla komendy, która nie zwraca rezultatu.
/// </summary>
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}