using System.IO;
using System.Reflection;
using CarsInfo.Infrastructure.DB.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CarsInfo.Infrastructure.DB
{
    public static class DbInitializer
    {
        private const string CreateTablesPath = @"/SQL/CreateTables.sql";
        private const string SeedDataPath = @"/SQL/SeedData.sql";
        private static readonly string Directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static SqlConnectionStringBuilder _connectionStringBuilder;

        public static void Initialize(string connectionString)
        {
            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            CreateDatabase();
            CreateSchema();
            ApplySeedData();
        }

        private static void CreateDatabase()
        {
            if (DatabaseAvailabilityChecker.DatabaseExists(_connectionStringBuilder))
            {
                return;
            }

            ExecuteNonQuery(_connectionStringBuilder.GetConnectionStringWithoutInitialCatalog(),
                $"CREATE DATABASE {_connectionStringBuilder.InitialCatalog}");
        }

        private static void CreateSchema()
        {
            if (DatabaseAvailabilityChecker.DatabaseExists(_connectionStringBuilder) && 
                DatabaseAvailabilityChecker.TablesExist(_connectionStringBuilder))
            {
                return;
            }

            var createTablesScript = File.ReadAllText(Directory + CreateTablesPath);
            ExecuteNonQuery(_connectionStringBuilder.ConnectionString, createTablesScript);
        }

        private static void ApplySeedData()
        {
            if (!DatabaseAvailabilityChecker.DatabaseExists(_connectionStringBuilder) || 
                !DatabaseAvailabilityChecker.TablesExist(_connectionStringBuilder) ||
                DatabaseAvailabilityChecker.SeedDataExist(_connectionStringBuilder))
            {
                return;
            }

            var seedDataScript = File.ReadAllText(Directory + SeedDataPath);
            ExecuteNonQuery(_connectionStringBuilder.ConnectionString, seedDataScript);
        }

        private static void ExecuteNonQuery(string connectionString, string commandText)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(commandText);
            }
        }
    }
}
