using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using CarsInfo.Infrastructure.DB.Extensions;

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

            ExecuteNonQuery(_connectionStringBuilder.GetDataSourceString(),
                $"CREATE DATABASE {_connectionStringBuilder.InitialCatalog}");
        }

        private static void CreateSchema()
        {
            if (!DatabaseExists() || TablesExist())
            {
                return;
            }

            var createTablesScript = File.ReadAllText(Directory + CreateTablesPath);
            ExecuteNonQuery(_connectionStringBuilder.ToString(), createTablesScript);
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
            ExecuteNonQuery(_connectionStringBuilder.ToString(), seedDataScript);
        }

        private static bool SeedDataExist()
        {
            var query = @$"USE {_connectionStringBuilder.InitialCatalog}; SELECT TOP 1 * FROM [User]";
            return QueryHasRows(_connectionStringBuilder.ToString(), query);
        }

        private static bool TablesExist()
        {
            var query = @$"USE {_connectionStringBuilder.InitialCatalog}; SELECT * FROM INFORMATION_SCHEMA.TABLES";
            return QueryHasRows(_connectionStringBuilder.ToString(), query);
        }

        private static bool DatabaseExists()
        {
            var query = $"SELECT * FROM master.dbo.sysdatabases WHERE name = '{_connectionStringBuilder.InitialCatalog}'";
            return QueryHasRows(_connectionStringBuilder.GetDataSourceString(), query);
        }

        private static void ExecuteNonQuery(string connectionString, string commandText)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var createTables = new SqlCommand(commandText, connection))
                {
                    createTables.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        
        private static bool QueryHasRows(string connectionString, string commandText)
        {
            var isExist = false;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(commandText, connection))
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        isExist = reader.HasRows;
                    }
                    connection.Close();
                }
            }

            return isExist;
        }
    }
}
