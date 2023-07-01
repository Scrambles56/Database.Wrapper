using Microsoft.Extensions.Configuration;
using Scrambles.Database.Wrapper.Exceptions;

namespace Scrambles.Database.Wrapper;

public interface IConnectionStringProvider
{
    string GetConnectionString(string connectionName);
}

public class ConfigurationConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public ConfigurationConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public string GetConnectionString(string connectionName)
    {
        return _configuration.GetConnectionString(connectionName)
            ?? throw new ConnectionStringNotFoundException(connectionName, "Connection string not found.");
    }
}