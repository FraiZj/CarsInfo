using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CarsInfo.WebApi.IntegrationTest.Configuration.Database
{
    public static class TestDatabaseConfiguration
    {
        public static string DefaultConnectionString => 
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;";
        
        public static string GetTempTestDbConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(DefaultConnectionString)
            {
                InitialCatalog = $"TestDb{RandomStringGenerator.GenerateRandom(10)}"
            };
            return connectionStringBuilder.ConnectionString;
        }

        public static void DropTempTestDbIfExists(SqlConnectionStringBuilder connectionStringBuilder)
        {
            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                var server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery("USE master; ");
                server.ConnectionContext.ExecuteNonQuery(
                    $"DROP DATABASE IF EXISTS {connectionStringBuilder.InitialCatalog};");
            }
        }
    }
}
