﻿using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace CarsInfo.DB
{
    public static class DbInitializer
    {
        private const string CreteDb = @"/SQL/CreateDb.sql";
        private const string CreateTables = @"/SQL/CreateTables.sql";

        public static void Initialize(string masterDbConnectionString)
        {
            if (CheckDatabaseExists(masterDbConnectionString))
            {
                return;
            }

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var createDbScript = File.ReadAllText(dir + CreteDb);
            var createTablesScript = File.ReadAllText(dir + CreateTables);
            
            using var con = new SqlConnection(masterDbConnectionString);
            using var createDb = new SqlCommand(createDbScript, con);
            using var createTables = new SqlCommand(createTablesScript, con);

            con.Open();
            createDb.ExecuteNonQuery();
            createTables.ExecuteNonQuery();
            con.Close();
        }

        private static bool CheckDatabaseExists(string masterDbConnectionString)
        {
            const string cmdText = "SELECT * FROM master.dbo.sysdatabases WHERE name ='CarsInfo'";
            var isExist = false;
            using var con = new SqlConnection(masterDbConnectionString);
            con.Open();
            using var cmd = new SqlCommand(cmdText, con);
            using var reader = cmd.ExecuteReader();
            isExist = reader.HasRows;
            con.Close();
            return isExist;
        }
    }
}
