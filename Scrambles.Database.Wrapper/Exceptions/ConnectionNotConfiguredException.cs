namespace Scrambles.Database.Wrapper.Exceptions;

public class ConnectionNotConfiguredException : Exception
{
    public DbAccessLevel AccessLevel { get; private set; }

    public ConnectionNotConfiguredException(DbAccessLevel accessLevel, string? message) : base(message)
    {
        AccessLevel = accessLevel;
    }
}