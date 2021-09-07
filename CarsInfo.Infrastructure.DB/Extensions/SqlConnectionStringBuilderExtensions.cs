using System.Data.SqlClient;

namespace CarsInfo.Infrastructure.DB.Extensions
{
    public static class SqlConnectionStringBuilderExtensions
    {
        public static string GetDataSourceString(this SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            return $"Data Source={sqlConnectionStringBuilder.DataSource};";
        }
    }
}