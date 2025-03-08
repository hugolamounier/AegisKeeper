using Microsoft.Data.SqlClient;

namespace AegisKeeper.Database.SQLServer;

public static class ConnectionStringValidator
{
    public static void Validate(string connectionString, out SqlConnectionStringBuilder sqlConnectionStringBuilder)
    {
        sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionStringBuilder.DataSource);
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionStringBuilder.InitialCatalog);
    }
}