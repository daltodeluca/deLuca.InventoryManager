using System.Data;

namespace deLuca.InventoryManager.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
