using CarsInfo.Infrastructure.DB.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CarsInfo.Infrastructure.DB
{
    internal static class DatabaseAvailabilityChecker
    {
        internal static bool SeedDataExist(SqlConnectionStringBuilder connectionStringBuilder)
        {
            var query = @$"USE {connectionStringBuilder.InitialCatalog}; SELECT TOP 1 * FROM [User]";
            return QueryHasRows(connectionStringBuilder.ConnectionString, query);
        }

        internal static bool TablesExist(SqlConnectionStringBuilder connectionStringBuilder)
        {
            var query = @$"USE {connectionStringBuilder.InitialCatalog}; SELECT * FROM INFORMATION_SCHEMA.TABLES";
            return QueryHasRows(connectionStringBuilder.ConnectionString, query);
        }

        internal static bool DatabaseExists(SqlConnectionStringBuilder connectionStringBuilder)
        {
            var query = $"SELECT * FROM master.dbo.sysdatabases WHERE name = '{connectionStringBuilder.InitialCatalog}'";
            return QueryHasRows(connectionStringBuilder.GetConnectionStringWithoutInitialCatalog(), query);
        }

        private static bool QueryHasRows(string connectionString, string commandText)
        {
            var isExist = false;

            using (var connection = new SqlConnection(connectionString))
            {
                var server = new Server(new ServerConnection(connection));
                var reader = server.ConnectionContext.ExecuteReader(commandText);
                isExist = reader.HasRows;
            }

            return isExist;
        }
    }
}
