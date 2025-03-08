using Microsoft.Data.SqlClient;

namespace AegisKeeper.Database.SQLServer;

public static class ConnectionStringValidator
{
    public static void Validate(string connectionString)
    {
        var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionStringBuilder.DataSource);
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionStringBuilder.InitialCatalog);
    }
}