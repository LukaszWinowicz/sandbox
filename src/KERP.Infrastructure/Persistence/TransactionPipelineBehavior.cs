using KERP.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KERP.Infrastructure.Persistence;

/// <summary>
/// A pipeline behavior that wraps the request handling in a database transaction.
/// This ensures that any database operations within a command handler are atomic.
/// </summary>
public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<TransactionPipelineBehavior<TRequest, TResponse>> _logger;

    public TransactionPipelineBehavior(AppDbContext dbContext, ILogger<TransactionPipelineBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
    {
        // Tylko komendy modyfikujące dane powinny być w transakcji.
        // Możemy to filtrować na podstawie nazwy lub implementacji interfejsu-znacznika.
        // Na razie dla prostoty zakładamy, że wszystkie `IRequest` (które nie są `IQuery` - to dodamy później) są transakcyjne.

        if (_dbContext.Database.CurrentTransaction is not null)
        {
            // Jeśli transakcja już istnieje (np. zagnieżdżona), po prostu kontynuuj.
            return await next();
        }

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("--> Beginning transaction {TransactionId} for {RequestName}", transaction.TransactionId, typeof(TRequest).Name);

            try
            {
                var response = await next();

                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("<-- Committed transaction {TransactionId}", transaction.TransactionId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "<-- Rolling back transaction {TransactionId} due to an exception", transaction.TransactionId);
                await transaction.RollbackAsync(cancellationToken);
                throw; // Rzucamy wyjątek dalej, aby mógł być obsłużony wyżej (np. przez globalny handler wyjątków)
            }
        });
    }
}