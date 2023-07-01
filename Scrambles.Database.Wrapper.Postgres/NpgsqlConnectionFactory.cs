using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace Scrambles.Database.Wrapper.Postgres;

public interface INpgsqlConnectionFactory : IDbConnectionFactory
{
}

public class NpgsqlConnectionFactory : INpgsqlConnectionFactory
{
    private readonly IConnectionStringProvider _connectionStringProvider;
    private readonly ConcurrentDictionary<string, DbDataSource> _dataSources = new();

    public NpgsqlConnectionFactory(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
    }

    public IDbConnection OpenConnection(string connectionName)
    {
        var dataSource = _dataSources.GetOrAdd(connectionName, GetDataSource);
        return dataSource.CreateConnection();
    }
    
    private DbDataSource GetDataSource(string connectionName)
    {
        var connectionString = _connectionStringProvider.GetConnectionString(connectionName);
        return new NpgsqlDataSourceBuilder(connectionString)
            .Build();
    }
}