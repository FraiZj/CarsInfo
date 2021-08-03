using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace CarsInfo.DB
{
    public static class DbInitializer
    {
        private const string CreateTablesPath = @"/SQL/CreateTables.sql";
        private const string SeedDataPath = @"/SQL/SeedData.sql";
        private static readonly string Directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Initialize(string masterDbConnectionString, string carsInfoConnectionString)
        {
            CreateDatabase(masterDbConnectionString);
            CreateSchema(carsInfoConnectionString);
            SeedData(carsInfoConnectionString);
        }

        private static void CreateDatabase(string masterDbConnectionString)
        {
            if (CheckDatabaseExists(masterDbConnectionString))
            {
                return;
            }

            using (var masterCon = new SqlConnection(masterDbConnectionString))
            {
                masterCon.Open();

                using (var createDb = new SqlCommand("CREATE DATABASE CarsInfo", masterCon))
                {
                    createDb.ExecuteNonQuery();
                }

                masterCon.Close();
            }
        }

        private static void CreateSchema(string carsInfoConnectionString)
        {
            if (!CheckDatabaseExists(carsInfoConnectionString))
            {
                return;
            }

            var createTablesScript = File.ReadAllText(Directory + CreateTablesPath);

            using (var carsInfoCon = new SqlConnection(carsInfoConnectionString))
            {
                carsInfoCon.Open();
                using (var createTables = new SqlCommand(createTablesScript, carsInfoCon))
                {
                    createTables.ExecuteNonQuery();
                }
                carsInfoCon.Close();
            }
        }

        private static void SeedData(string carsInfoConnectionString)
        {
            if (TablesExist(carsInfoConnectionString))
            {
                return;
            }

            var seedDataScript = File.ReadAllText(Directory + SeedDataPath);

            using (var carsInfoCon = new SqlConnection(carsInfoConnectionString))
            {
                carsInfoCon.Open();
                using (var seedData = new SqlCommand(seedDataScript, carsInfoCon))
                {
                    seedData.ExecuteNonQuery();
                }
                carsInfoCon.Close();
            }
        }

        private static bool TablesExist(string carsInfoConString)
        {
            const string cmdText = @"SELECT TOP 1 * FROM [User]";
            var isExist = false;

            using (var con = new SqlConnection(carsInfoConString))
            {
                con.Open();
                using (var cmd = new SqlCommand(cmdText, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        isExist = reader.HasRows;
                    }
                }
                con.Close();
            }

            return isExist;
        }

        private static bool CheckDatabaseExists(string masterDbConnectionString)
        {
            const string cmdText = "SELECT * FROM master.dbo.sysdatabases WHERE name = 'CarsInfo'";
            var isExist = false;

            using (var con = new SqlConnection(masterDbConnectionString))
            {
                using (var cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        isExist = reader.HasRows;
                    }
                    con.Close();
                }
            }

            return isExist;
        }
    }
}
