using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CarsInfo.WebApi.IntegrationTest
{
    public abstract class AbstractIntegrationTest
    {
        private const string ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;";

        protected readonly HttpClient TestClient;
        
        protected AbstractIntegrationTest()
        {
            var applicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((_, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                ["ConnectionStrings:CarsInfoDb"] = ConnectionString
                            });
                    });
                });
            TestClient = applicationFactory.CreateClient();
        }

        public virtual async Task<ToggleFavoriteStatus> AddCarToFavoriteAsync(int carId)
        {
            var response = await TestClient.PostAsJsonAsync($"{carId}/favorite", new {});
            return await response.Content.ReadFromJsonAsync<ToggleFavoriteStatus>();
        }

        public virtual async Task AuthenticateAdminAsync()
        {
            var response = await TestClient.PostAsJsonAsync("/login", new LoginViewModel
            {
                Email = "admin@email.com",
                Password = "admin@email.com"
            });

            var loginResponse = await response.Content.ReadFromJsonAsync<AuthViewModel>();
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "bearer", loginResponse?.AccessToken);
        }

        public virtual async Task AuthenticateUserAsync()
        {
            var response = await TestClient.PostAsJsonAsync("/login", new LoginViewModel
            {
                Email = "integration@test.com",
                Password = "integration@test.com"
            });

            if (!response.IsSuccessStatusCode)
            {
                response = await TestClient.PostAsJsonAsync("/register", new RegisterViewModel
                {
                    FirstName = "Integration",
                    LastName = "Test",
                    Email = "integration@test.com",
                    Password = "integration@test.com"
                });
            }

            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthViewModel>();
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "bearer", registrationResponse?.AccessToken);
        }
    }
}
