using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CarsInfo.Infrastructure.DB.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly SqlConnectionStringBuilder _connectionStringBuilder;

        public DatabaseHealthCheck(string connectionString)
        {
            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new())
        {
            try
            {
                if (!DatabaseAvailabilityChecker.DatabaseExists(_connectionStringBuilder))
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy("Database does not exist"));
                }

                if (!DatabaseAvailabilityChecker.TablesExist(_connectionStringBuilder))
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy("Database does not have tables"));
                }

                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
    }
}
