using Microsoft.Data.SqlClient;

namespace CarsInfo.Infrastructure.DB.Extensions
{
    public static class SqlConnectionStringBuilderExtensions
    {
        private const string InitialCatalog = "Initial Catalog";
        
        public static string GetConnectionStringWithoutInitialCatalog(
            this SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            var copy = new SqlConnectionStringBuilder(sqlConnectionStringBuilder.ConnectionString);
            copy.Remove(InitialCatalog);
            return copy.ToString();
        }
    }
}