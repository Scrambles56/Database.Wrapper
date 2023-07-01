namespace Scrambles.Database.Wrapper.Postgres;

public class NpgsqlDatabaseWrapper : DatabaseWrapper, INpgsqlDatabaseWrapper
{
    public NpgsqlDatabaseWrapper(INpgsqlConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }
}

public interface INpgsqlDatabaseWrapper: IDatabaseWrapper
{
}