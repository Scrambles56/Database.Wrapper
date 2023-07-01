namespace Scrambles.Database.Wrapper.Exceptions;

public class ConnectionStringNotFoundException : Exception
{
    public string ConnectionName { get; private set; }

    public ConnectionStringNotFoundException(string connectionName, string? message) 
        : base(message)
    {
        ConnectionName = connectionName ?? throw new ArgumentNullException(nameof(connectionName));
    }
}