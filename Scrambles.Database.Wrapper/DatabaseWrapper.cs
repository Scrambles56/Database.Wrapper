using System.Data;
using Dapper;
using Scrambles.Database.Wrapper.Exceptions;

namespace Scrambles.Database.Wrapper;

public interface IDatabaseWrapper
{
}

public class DatabaseWrapper : IDatabaseWrapper
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly Dictionary<DbAccessLevel, string> _accessLevelConnectionLevels = new();

    public DatabaseWrapper(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Configure(string connectionName, DbAccessLevel accessLevel)
    {
        _accessLevelConnectionLevels.Add(accessLevel, connectionName);
    }

    public Task<IEnumerable<T>> Query<T>(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    )
    {
        if (!_accessLevelConnectionLevels.TryGetValue(accessLevel, out var connectionName))
        {
            throw new ConnectionNotConfiguredException(accessLevel, "Connection not configured for access level.");
        }

        using var connection = _connectionFactory.OpenConnection(connectionName);
        var command = new CommandDefinition(
            sql,
            parameters,
            commandTimeout: queryTimeout,
            commandType: CommandType.Text,
            cancellationToken: cancellationToken
        );

        return connection.QueryAsync<T>(command);
    }

    public async Task<List<T>> QueryAsList<T>(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    ) =>
        (await Query<T>(sql, parameters, accessLevel, queryTimeout, cancellationToken).ConfigureAwait(false)).ToList();

    public async Task<T?> QueryFirstOrDefault<T>(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    ) => 
        (await Query<T>(sql, parameters, accessLevel, queryTimeout, cancellationToken).ConfigureAwait(false)).FirstOrDefault();
    
    public async Task<T> QueryFirst<T>(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    ) => 
        (await Query<T>(sql, parameters, accessLevel, queryTimeout, cancellationToken).ConfigureAwait(false)).First();
    
    public async Task<T?> QuerySingleOrDefault<T>(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    ) => 
        (await Query<T>(sql, parameters, accessLevel, queryTimeout, cancellationToken).ConfigureAwait(false)).SingleOrDefault();
    
    public async Task<T> QuerySingle<T>(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    ) => 
        (await Query<T>(sql, parameters, accessLevel, queryTimeout, cancellationToken).ConfigureAwait(false)).Single();

    public Task<int> Execute(
        string sql,
        object? parameters = null,
        DbAccessLevel accessLevel = DatabaseConstants.DefaultAccessLevel,
        int queryTimeout = DatabaseConstants.QueryTimeout,
        CancellationToken cancellationToken = default
    )
    {
        if (!_accessLevelConnectionLevels.TryGetValue(accessLevel, out var connectionName))
        {
            throw new ConnectionNotConfiguredException(accessLevel, "Connection not configured for access level.");
        }

        using var connection = _connectionFactory.OpenConnection(connectionName);
        var command = new CommandDefinition(
            sql,
            parameters,
            commandTimeout: queryTimeout,
            commandType: CommandType.Text,
            cancellationToken: cancellationToken
        );

        return connection.ExecuteAsync(command);   
    }
    
}