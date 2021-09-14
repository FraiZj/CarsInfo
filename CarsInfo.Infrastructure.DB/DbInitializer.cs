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
            if (DatabaseExists())
            {
                return;
            }

            ExecuteNonQuery(_connectionStringBuilder.GetConnectionStringWithoutInitialCatalog(),
                $"CREATE DATABASE {_connectionStringBuilder.InitialCatalog}");
        }

        private static void CreateSchema()
        {
            if (DatabaseExists() && TablesExist())
            {
                return;
            }

            var createTablesScript = File.ReadAllText(Directory + CreateTablesPath);
            ExecuteNonQuery(_connectionStringBuilder.ConnectionString, createTablesScript);
        }

        private static void ApplySeedData()
        {
            if (!DatabaseExists() || 
                !TablesExist() || 
                SeedDataExist())
            {
                return;
            }

            var seedDataScript = File.ReadAllText(Directory + SeedDataPath);
            ExecuteNonQuery(_connectionStringBuilder.ConnectionString, seedDataScript);
        }

        private static bool SeedDataExist()
        {
            var query = @$"USE {_connectionStringBuilder.InitialCatalog}; SELECT TOP 1 * FROM [User]";
            return QueryHasRows(_connectionStringBuilder.ConnectionString, query);
        }

        private static bool TablesExist()
        {
            var query = @$"USE {_connectionStringBuilder.InitialCatalog}; SELECT * FROM INFORMATION_SCHEMA.TABLES";
            return QueryHasRows(_connectionStringBuilder.ConnectionString, query);
        }

        private static bool DatabaseExists()
        {
            var query = $"SELECT * FROM master.dbo.sysdatabases WHERE name = '{_connectionStringBuilder.InitialCatalog}'";
            return QueryHasRows(_connectionStringBuilder.GetConnectionStringWithoutInitialCatalog(), query);
        }

        private static void ExecuteNonQuery(string connectionString, string commandText)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(commandText);
            }
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
