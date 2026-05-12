using System.Data;
using Microsoft.Data.SqlClient;

namespace deLuca.InventoryManager.Infrastructure.Data;

public sealed class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
