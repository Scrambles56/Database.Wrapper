using System.Data;

namespace Scrambles.Database.Wrapper;

public interface IDbConnectionFactory
{
    public IDbConnection OpenConnection(string connectionName);
}