using KERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace KERP.Infrastructure.Persistence.Interceptors;

public class FactoryTenantInterceptor : DbCommandInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private const string TenantPlaceholder = "_DYNAMIC";

    public FactoryTenantInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        RewriteCommandText(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        RewriteCommandText(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    // Można też nadpisać NonQueryExecutingAsync i ScalarExecutingAsync w ten sam sposób
    private void RewriteCommandText(DbCommand command)
    {
        // Sprawdzamy, czy zapytanie zawiera nasz placeholder
        if (command.CommandText.Contains(TenantPlaceholder))
        {
            var factoryId = _currentUserService.SelectedFactoryId;
            if (factoryId.HasValue)
            {
                // Podmieniamy placeholder na aktualne ID fabryki
                command.CommandText = command.CommandText.Replace(TenantPlaceholder, $"_{factoryId.Value}");
            }
            else
            {
                // Jeśli nie wybrano fabryki, rzucamy błąd, aby uniknąć zapytania do nieistniejącej tabeli
                throw new InvalidOperationException($"Cannot query tenanted table without a selected factory. Query: {command.CommandText}");
            }
        }
    }
}
