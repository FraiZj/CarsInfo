using System;
using System.Collections.Generic;
using System.Net.Http;
using CarsInfo.WebApi.IntegrationTest.Configuration.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CarsInfo.WebApi.IntegrationTest.Configuration
{
    public class TempHttpClientWrapper : IDisposable
    {
        private bool _disposed;
        private readonly SqlConnectionStringBuilder _connectionStringBuilder;
        public HttpClient HttpClient { get; }

        private TempHttpClientWrapper(string connectionString)
        {
            connectionString ??= TestDatabaseConfiguration.GetTempTestDbConnectionString();
            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var applicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((_, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                ["ConnectionStrings:CarsInfoDb"] = _connectionStringBuilder.ConnectionString
                            });
                    });
                });
            HttpClient = applicationFactory.CreateClient();
        }

        ~TempHttpClientWrapper() => Dispose(false);
        
        public static TempHttpClientWrapper Create(string connectionString = null)
        {
            return new TempHttpClientWrapper(connectionString);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            HttpClient.Dispose();
            TestDatabaseConfiguration.DropTempTestDbIfExists(_connectionStringBuilder);

            _disposed = true;
        }
    }
}
